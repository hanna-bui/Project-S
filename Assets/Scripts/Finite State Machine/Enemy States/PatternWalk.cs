using System;
using System.Collections.Generic;
using Characters;
using Characters.Enemies;
using UnityEngine;
using Characters.Enemy;
using Action = Characters.Enemies.Action;
using MO = Characters.Enemies.Enemy.MovementOptions;
// ReSharper disable Unity.PerformanceCriticalCodeNullComparison

namespace Finite_State_Machine.Enemy_States
{
    public class PatternWalk : State
    {
        private List<Vector3> roadPath;
        
        private const int Scale = 20;

        private int index;
        
        public PatternWalk()
        {
            interval = 0f;
        }
        
        protected override void Initialize(MoveableObject agent)
        {
            var enemy = agent as Enemy;
            switch (enemy.MovementStyle)
            {
                case MO.Plus:
                    SetupPlusOrSide(enemy);
                    break;
                case MO.Side:
                    SetupPlusOrSide(enemy);
                    break;
                case MO.Random:
                    roadPath = null;
                    break;
                case MO.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            enemy.SetAnimations(Action.Jump);
            CurrentStatus = StateStatus.Executing;
        }

        protected override void Executing(MoveableObject agent)
        {
            var enemy = agent as Enemy;
            Move(enemy);
        }

        protected override void Completed(MoveableObject agent)
        {
            var enemy = agent as Enemy;
            enemy.SetAnimations(Action.Idle);
            enemy.ChangeState(new EnemyIdle());
        }

        private void Move(Enemy agent)
        {
            switch (agent.MovementStyle)
            {
                case MO.Plus:
                    PlusOrSideMovement(agent);
                    break;
                case MO.Side:
                    PlusOrSideMovement(agent);
                    break;
                case MO.Random:
                    RandomMovement(agent);
                    break;
                case MO.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Movement Pattern

        private void SetupPlusOrSide(Enemy agent)
        {
            var grid = agent.grid;
            
            roadPath = new List<Vector3>();
            var origin = agent.Origin;
            var pos = agent.Position();
            
            roadPath.Add(origin);
            
            if (agent.MovementStyle is MO.Plus)
            {
                var y = 20f;
                
                while (!grid.IsWalkable(pos + new Vector3(0, y, 0)) && y > 0)
                {
                    y -= 1f;
                }
                
                roadPath.Add(origin + new Vector3(0, y, 0));

                roadPath.Add(origin);
            }

            var x = 20f;
            while (!grid.IsWalkable(pos + new Vector3(x, 0, 0)) && x > 0)
            {
                x -= 1f;
            }

            roadPath.Add(origin + new Vector3(x, 0, 0));

            roadPath.Add(origin);

            if (agent.MovementStyle is MO.Plus)
            {
                var y = -20f;
                while (!grid.IsWalkable(pos + new Vector3(0, y, 0)) && y < 0)
                {
                    y += 1f;
                }

                roadPath.Add(origin + new Vector3(0, y, 0));

                roadPath.Add(origin);
            }

            x = -20f;
            while (!grid.IsWalkable(pos + new Vector3(x, 0, 0)) && x < 0)
            {
                x += 1f;
            }

            roadPath.Add(origin + new Vector3(x, 0, 0));
        }

        private void PlusOrSideMovement(Enemy agent)
        {
            var pos = agent.Position();
        
            var newPosition = roadPath[index];
        
            if (pos != roadPath[index])
            {
                var towards = Vector3.MoveTowards(pos, newPosition, agent.SPE * 10 * Time.deltaTime);
                agent.SetPosition(towards);
            }
            else
            {
                index++;
                if (index == roadPath.Count)
                {
                    index = 0;
                }
            }
        }

        private void RandomMovement(Enemy agent)
        {
            var grid = agent.grid;
            
            var pos = agent.Position();

            if (roadPath == null)
            {
                var newPos = grid.GetRandomCoords();
                roadPath = grid.CreatePath(pos, newPos);
            }

            if (roadPath != null)
            {
                var targetLocation = roadPath[0];

                if (agent.IsAtPosition(targetLocation)) roadPath.RemoveAt(0);

                if (roadPath.Count == 0)
                {
                    roadPath = null;
                }

                var towards = Vector3.MoveTowards(pos, targetLocation, agent.SPE * 10 * Time.deltaTime);
                agent.SetPosition(towards);
            }
        }

        #endregion Movement Pattern

    }
}