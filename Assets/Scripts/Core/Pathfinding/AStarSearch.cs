using System;
using System.Collections.Generic;
using System.Linq;
using Core.Util;
using UnityEngine;

namespace Core.Pathfinding {
    public class AStarSearch {

        private static readonly float Sqrt2 = Mathf.Sqrt(2);

        private readonly Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        private readonly Dictionary<Vector2Int, float> costSoFar = new Dictionary<Vector2Int, float>();
        private readonly PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();

        private readonly BoardPassabilityGrid graph;

        private readonly Vector2Int start;

        private readonly IList<Vector2Int> goals;
        private readonly IList<Vector2Int> foundGoals = new List<Vector2Int>();
        private readonly IList<Vector2Int> blacklistedGoals = new List<Vector2Int>();

        private FindOptions options;

        private float costFromStartMultiplier;

        private float costFromStartMultiplierDistanceDivider = 100;

        public AStarSearch(BoardPassabilityGrid graph, Vector3Int start) : this(graph, start, new[] {start}) {
        }

        public AStarSearch(BoardPassabilityGrid graph, Vector3Int start, IEnumerable<Vector3Int> goals) {
            this.graph = graph;
            this.start = (Vector2Int) start;
            this.goals = goals.Select(v => (Vector2Int) v).ToList();

            Init();
        }

        public void SetOptions(FindOptions options) {
            this.options = options;
        }

        private void Init() {
            // Add the starting location to the frontier with a priority of 0
            frontier.Enqueue(start, 0f);
            cameFrom.Add(start, start); // is set to start, None in example
            costSoFar.Add(start, 0f);

            // Filter and refill goals
            PrepareGoals();

            costFromStartMultiplier = GetCostFromStartMultiplier(start, goals);
        }

        private float GetCostFromStartMultiplier(Vector2Int start, IList<Vector2Int> goals) {
            if (goals.Count == 0) {
                return 0;
            }

            var averageDistance = AverageHeuristic(start, goals);

            return 1f / (1 + averageDistance / costFromStartMultiplierDistanceDivider);
        }

        private void PrepareGoals() {
            IList<Vector2Int> removedGoals;

            var limit = 10;

            do {
                removedGoals = FilterGoals();

                if (goals.Count == 0) {
                    AddAdjacentGoals(removedGoals);
                }
            } while (limit-- > 0 && removedGoals.Count > 0);
        }

        private IList<Vector2Int> FilterGoals() {
            var removedGoals = new List<Vector2Int>();

            for (var i = goals.Count - 1; i >= 0; i--) {
                var goal = goals[i];

                if (!CanBeUsedAsGoal(goal)) {
                    goals.RemoveAt(i);
                    blacklistedGoals.Add(goal);
                    removedGoals.Add(goal);
                }
            }

            return removedGoals;
        }

        private void AddAdjacentGoals(IList<Vector2Int> removedGoals) {
            foreach (var removedGoal in removedGoals) {
                AddAdjacentGoals(removedGoal);
            }
        }

        private void AddAdjacentGoals(Vector2Int removedGoal) {
            var adjacents = graph.GetAvailableAdjacents(removedGoal);

            foreach (var adjacent in adjacents) {
                if (graph.IsDirectlyPassable(adjacent) && !blacklistedGoals.Contains(adjacent)) {
                    goals.Add(adjacent);
                }
            }
        }

        private void IterateUntilFrontier() {

            // add the cross product of the start to goal and tile to goal vectors
            // Vector3 startToGoalV = Vector3.Cross(start.vector3,goal.vector3);
            // Vector2Int startToGoal = new Vector2Int(startToGoalV);
            // Vector3 neighborToGoalV = Vector3.Cross(neighbor.vector3,goal.vector3);

            // frontier is a List of key-value pairs:
            // Vector2Int, (float) priority

            var limit = 1000;

            while (frontier.Count > 0) {
                // Get the Vector2Int from the frontier that has the lowest
                // priority, then remove that Vector2Int from the frontier
                var current = frontier.Dequeue();

                if (limit-- <= 0) {
                    throw new Exception("Limit exceeded");
                }

                // Это должно быть до выполнения break, чтобы при повторном обращении имелись данные,
                // на основе которых можно было бы продолжить поиск пути.
                EnqueueNeighbours(current);
            }
        }

        private void IterateUntilFirstGoal() {

            if (goals.Count == 0) {
                return;
            }

            // add the cross product of the start to goal and tile to goal vectors
            // Vector3 startToGoalV = Vector3.Cross(start.vector3,goal.vector3);
            // Vector2Int startToGoal = new Vector2Int(startToGoalV);
            // Vector3 neighborToGoalV = Vector3.Cross(neighbor.vector3,goal.vector3);

            // frontier is a List of key-value pairs:
            // Vector2Int, (float) priority

            var limit = 2000;

            while (frontier.Count > 0f) {
                // Get the Vector2Int from the frontier that has the lowest
                // priority, then remove that Vector2Int from the frontier
                var current = frontier.Dequeue();

                if (limit-- <= 0) {
                    throw new Exception("Limit exceeded");
                }

                // Это должно быть до выполнения break, чтобы при повторном обращении имелись данные,
                // на основе которых можно было бы продолжить поиск пути.
                EnqueueNeighbours(current);

                // If we're at the goal Vector2Int, stop looking.
                if (goals.Contains(current)) {
                    foundGoals.Add(current);
                    break;
                }
            }
        }

        private void EnqueueNeighbours(Vector2Int current) {
            var neighbors = graph.GetAvailableNeighbors(current);

            // Neighbors will return a List of valid tile Locations
            // that are next to, diagonal to, above or below current
            foreach (var neighbor in neighbors) {
                // If neighbor is diagonal to current, graph.Cost(current,neighbor)
                // will return Sqrt(2). Otherwise it will return only the cost of
                // the neighbor, which depends on its type, as set in the TileType enum.
                // So if this is a normal floor tile (1) and it's neighbor is an
                // adjacent (not diagonal) floor tile (1), costFromStart will be 2,
                // or if the neighbor is diagonal, 1+Sqrt(2). And that will be the
                // value assigned to costSoFar[neighbor] below.
                var costFromStart = costSoFar[current] + graph.Cost(current, neighbor);

                if (!options.commonPredicate(neighbor, costFromStart)) {
                    continue;
                }

                // If we can't pass that cell but cell is allowed as target,
                // just store values in costSoFar and cameFrom without enqueueing to frontier
                if (!options.passabilityPredicate(neighbor)) {
                    if (options.targetPredicate(neighbor)) {
                        costSoFar.Add(neighbor, costFromStart);
                        cameFrom.Add(neighbor, current);
                    }

                    continue;
                }
                
                // If there's no cost assigned to the neighbor yet, or if the new
                // cost is lower than the assigned one, add costFromStart for this neighbor
                if (costSoFar.ContainsKey(neighbor) && !(costFromStart < costSoFar[neighbor])) {
                    continue;
                }

                // If we're replacing the previous cost, remove it
                if (costSoFar.ContainsKey(neighbor)) {
                    costSoFar.Remove(neighbor);
                    cameFrom.Remove(neighbor);
                }

                costSoFar.Add(neighbor, costFromStart);
                cameFrom.Add(neighbor, current);

                var priority = costFromStart * costFromStartMultiplier + AverageHeuristic(neighbor, goals);
                frontier.Enqueue(neighbor, priority);
            }
        }

        public IEnumerable<Vector2Int> FindPositions(FindOptions options) {
            SetOptions(options);
            
            IterateUntilFrontier();

            cameFrom.Remove(start);

            return cameFrom.Keys
                    .Where(targetPosition => {
                        if (!options.targetPredicate(targetPosition)) {
                            return false;
                        }

                        var path = TryCollectPathFrom(targetPosition);

                        if (path == null) {
                            return false;
                        }

                        return path.Skip(1).SkipLast(1).All(v => options.passabilityPredicate(v));
                    });
        }

        // Return a List of Locations representing the found path
        public IEnumerable<Vector2Int> FindPath() {
            IterateUntilFirstGoal();

            while (goals.Count > 0 && foundGoals.Count > 0 && !CanBeUsedAsGoal(foundGoals.Last())) {
                goals.Remove(foundGoals.Last());
                IterateUntilFirstGoal();
            }

            if (foundGoals.Count == 0) {
                return Array.Empty<Vector2Int>();
            }

            var path = FindPathFromGoal(foundGoals.Last(), foundGoals.Last());

            return path.Count > 0 ? path : Array.Empty<Vector2Int>();
        }

        public IList<Vector2Int> FindPathFromGoal(Vector2Int goal, Vector2Int to) {
            var path = new[] {goal}
                    .Where(CanBeUsedAsGoal)
                    .Select(TryCollectPathFrom)
                    .FirstOrDefault(p => p != null);

            if (path == null) {
                var position = FindNearestReachablePosition(to);
                path = TryCollectPathFrom(position);
            }

            return path ?? new List<Vector2Int>();
        }

        private Vector2Int FindNearestReachablePosition(Vector2Int to) {
            return costSoFar
                    .Select(pair => pair.Key)
                    .Where(CanBeUsedAsGoal)
                    .OrderBy(from => Vector2Int.Distance(from, to))
                    .FirstOrDefault();
        }

        private bool CanBeUsedAsGoal(Vector2Int cell) {
            return graph.IsInBounds(cell) && !graph.IsUsedAsGoal(cell) && graph.IsFreeOfIdleObjects(cell);
        }

        private IList<Vector2Int> TryCollectPathFrom(Vector2Int position) {
            var path = new List<Vector2Int>();
            var current = position;

            while (!current.Equals(start)) {
                if (!cameFrom.ContainsKey(current)) {
                    return null;
                }

                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }

        public static float Heuristic(Vector2Int a, Vector2Int b) {
            var dx = Mathf.Abs(a.x - b.x);
            var dy = Mathf.Abs(a.y - b.y);

            return Mathf.Max(dx, dy);
        }

        private static float AverageHeuristic(Vector2Int a, IList<Vector2Int> others) {
            return others.Select(b => Heuristic(a, b)).Average();
        }

        public readonly struct FindOptions {

            public readonly Func<Vector2Int, float, bool> commonPredicate;

            public readonly Func<Vector2Int, bool> passabilityPredicate;

            public readonly Func<Vector2Int, bool> targetPredicate;

            public FindOptions(Func<Vector2Int, float, bool> commonPredicate) {
                this.commonPredicate = commonPredicate;
                passabilityPredicate = _ => true;
                targetPredicate = _ => true;
            }

            public FindOptions(
                    Func<Vector2Int, float, bool> commonPredicate,
                    Func<Vector2Int, bool> passabilityPredicate,
                    Func<Vector2Int, bool> targetPredicate
            ) {
                this.commonPredicate = commonPredicate;
                this.passabilityPredicate = passabilityPredicate;
                this.targetPredicate = targetPredicate;
            }
        }
    }
}