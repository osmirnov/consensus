﻿using System.Collections.Generic;
using System.Linq;
using Consensus.FastBFT.Replicas;

namespace Consensus.FastBFT.Infrastructure
{
    public class ReplicaTopology
    {
        public static void Discover(ReplicaBase parentReplica, IEnumerable<Replica> secondaryReplicas)
        {
            if (parentReplica == null || secondaryReplicas.Any() == false) return;

            var leftReplicas = secondaryReplicas.Where(r => r.Id != parentReplica.Id);
            var rightReplicas = leftReplicas.Skip(1);
            var restReplicas = rightReplicas.Skip(1);

            if (leftReplicas.Any())
            {
                var leftReplica = leftReplicas.FirstOrDefault();

                leftReplica.ParentReplica = parentReplica;

                parentReplica.ChildReplicas.Add(leftReplica);

                Discover(leftReplica, restReplicas.Take(restReplicas.Count() / 2));
            }

            if (rightReplicas.Any())
            {
                var rightReplica = rightReplicas.FirstOrDefault();

                rightReplica.ParentReplica = parentReplica;

                parentReplica.ChildReplicas.Add(rightReplica);

                Discover(rightReplica, restReplicas.Skip(restReplicas.Count() / 2));
            }
        }

        public static IReadOnlyCollection<ReplicaBase> GetActiveReplicas(ReplicaBase replica)
        {
            var activeReplicas = new List<ReplicaBase>();

            foreach (var childReplica in replica.ChildReplicas)
            {
                activeReplicas.Add(childReplica);
                GetActiveReplicas(childReplica, activeReplicas);
            }

            return activeReplicas;
        }

        private static void GetActiveReplicas(ReplicaBase replica, ICollection<ReplicaBase> activeReplicas)
        {
            foreach (var childReplica in replica.ChildReplicas)
            {
                activeReplicas.Add(childReplica);
                GetActiveReplicas(childReplica, activeReplicas);
            }
        }
    }
}