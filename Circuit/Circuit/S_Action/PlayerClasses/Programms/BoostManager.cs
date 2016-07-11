using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.S_Action.PlayerClasses.Programms.Delete;
using Circuit.Utilities;
using Microsoft.Xna.Framework;
using Circuit.S_Action.PlayerClasses.Programms.Optimize;
using Circuit.S_Action.PlayerClasses.Programms.Firewall;
using Circuit.S_Action.PlayerClasses.Programms.GoTo;

namespace Circuit.S_Action.PlayerClasses.Programms
{
    class BoostManager
    {
        Delete.DeleteBoost mDeleteSlot;
        Optimize.OptimizeBoost mOptimizePSlot;
        Optimize.OptimizeNegativeBoost mOptimizeNSlot;
        Firewall.FirewallBoost mFirewallSlot;
        GoTo.GoToBoost mGoToSlot;

        public event EventHandler StartDeleteBoost;
        public event EventHandler EndDeleteBoost;

        public event EventHandler StartOptimizePositiveBoost;
        public event EventHandler EndOptimizePositiveBoost;

        public event EventHandler StartOptimizeNegativeBoost;
        public event EventHandler EndOptimizeNegativeBoost;

        public event EventHandler StartFirewallBoost;
        public event EventHandler EndFirewallBoost;

        public event EventHandler StartGoToBoost;
        public event EventHandler EndGoToBoost;

        public bool Empty(Data.ProgrammType _type)
        {
            if (_type == Data.ProgrammType.DELETE)
                return mDeleteSlot == null;

            if (_type == Data.ProgrammType.OPTIMIZE)
                return mOptimizePSlot == null;

            if (_type == Data.ProgrammType.FIREWALL)
                return mFirewallSlot == null;

            if (_type == Data.ProgrammType.GOTO)
                return mGoToSlot == null;

            if (_type == Data.ProgrammType.OPTIMIZE_N)
                return mOptimizeNSlot == null;

            if (_type == Data.ProgrammType.DEFAULT)
                return mOptimizeNSlot == null;

            throw new Exception();
        }

        public BoostManager()
        { }

        public void Update(GameTime _GT)
        {
            if (mDeleteSlot != null)
            {
                mDeleteSlot.Update(_GT);

                if (mDeleteSlot.BurnedOut)
                {
                    mDeleteSlot = null;

                    if (EndDeleteBoost != null)
                        EndDeleteBoost(null, EventArgs.Empty);
                }
            }

            if (mOptimizePSlot != null)
            {
                mOptimizePSlot.Update(_GT);

                if (mOptimizePSlot.BurnedOut)
                {
                    mOptimizePSlot = null;
                    if (EndOptimizePositiveBoost != null)
                        EndOptimizePositiveBoost(null, EventArgs.Empty);
                }
            }

            if (mOptimizeNSlot != null)
            {
                mOptimizeNSlot.Update(_GT);

                if (mOptimizeNSlot.BurnedOut)
                {
                    mOptimizeNSlot = null;
                    if (EndOptimizeNegativeBoost != null)
                        EndOptimizeNegativeBoost(null, EventArgs.Empty);
                }
            }

            if (mFirewallSlot != null)
            {
                mFirewallSlot.Update(_GT);

                if (mFirewallSlot.BurnedOut)
                {
                    mFirewallSlot = null;
                    if (EndFirewallBoost != null)
                        EndFirewallBoost(null, EventArgs.Empty);
                }
            }

            if (mGoToSlot != null)
            {
                mGoToSlot.Update(_GT);

                if (mGoToSlot.BurnedOut)
                {
                    mGoToSlot = null;
                    if (EndGoToBoost != null)
                        EndGoToBoost(null, EventArgs.Empty);
                }
            }
        }

        public void AddBoost(Boost _boost)
        {
            // Delete
            if (_boost is DeleteBoost &&
               mDeleteSlot == null)
            {
                mDeleteSlot = (DeleteBoost)_boost;
                mDeleteSlot.Start();

                if (StartDeleteBoost != null)
                    StartDeleteBoost(null, EventArgs.Empty);
            }

            //Optimize Boost
            if (_boost is OptimizeBoost && mOptimizePSlot == null)
            {
                mOptimizePSlot = (OptimizeBoost)_boost;
                mOptimizePSlot.Start();

                if (StartOptimizePositiveBoost != null) StartOptimizePositiveBoost(null, EventArgs.Empty);
            }

            //Optimize Shot Attribute
            if (_boost is OptimizeNegativeBoost && mOptimizeNSlot == null)
            {
                mOptimizeNSlot = (OptimizeNegativeBoost)_boost;
                mOptimizeNSlot.Start();

                if (StartOptimizeNegativeBoost != null) StartOptimizeNegativeBoost(null, EventArgs.Empty);
            }

            // Firewall
            if (_boost is FirewallBoost && mFirewallSlot == null)
            {
                mFirewallSlot = (FirewallBoost)_boost;
                mFirewallSlot.Start();

                if (StartFirewallBoost != null) StartFirewallBoost(null, EventArgs.Empty);
            }

            // GoTo
            if (_boost is GoToBoost && mGoToSlot == null)
            {
                mGoToSlot = (GoToBoost)_boost;
                mGoToSlot.Start();

                if (StartGoToBoost != null) StartGoToBoost(null, EventArgs.Empty);
            }
        }
    }
}
