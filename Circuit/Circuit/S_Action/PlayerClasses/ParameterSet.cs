using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circuit.Utilities;
using Circuit.S_Action.PlayerClasses.Programms;

namespace Circuit.S_Action.PlayerClasses
{
    class ParameterSet
    {
        float mSpeed;
        float mJumpheight;
        public event EventHandler SetDeleteCollision;
        public event EventHandler DeSetDeleteCollision;

        public event EventHandler SetOptimizeSpeedBuff;
        public event EventHandler DeSetOptimizeSpeedBuff;

        public event EventHandler SetOptimizeSpeedDeBuff;
        public event EventHandler DeSetOptimizeSpeedDeBuff;

        public event EventHandler SetGotoJumpBuff;
        public event EventHandler DeSetGotoJumpBuff;

        public event EventHandler SetFirewallInvulnBuff;
        public event EventHandler DeSetFirewallInvulnBuff;

        public float Speed
        {
            get { return mSpeed; }
        }

        public ParameterSet(BoostManager _bm)
        {
            _bm.StartDeleteBoost += new EventHandler(OnStartDelete);
            _bm.StartOptimizePositiveBoost += new EventHandler(OnStartPosOptimize);
            _bm.StartOptimizeNegativeBoost += new EventHandler(OnStartNegOptimize);
            _bm.StartGoToBoost += new EventHandler(OnStartGoto);
            _bm.StartFirewallBoost += new EventHandler(OnStartFirewall);

            _bm.EndDeleteBoost += new EventHandler(OnEndDelete);
            _bm.EndOptimizeNegativeBoost += new EventHandler(OnEndNegOptimize);
            _bm.EndOptimizePositiveBoost += new EventHandler(OnEndPosOptimize);
            _bm.EndGoToBoost += new EventHandler(OnEndGoto);
            _bm.EndFirewallBoost += new EventHandler(OnEndFirewall);
        }

        private void OnStartDelete(object _sender, EventArgs _e)
        {
            if (SetDeleteCollision != null)
                SetDeleteCollision(null, EventArgs.Empty);
        }

        private void OnStartPosOptimize(object _sender, EventArgs _e)
        {
            if (SetOptimizeSpeedBuff != null)
                SetOptimizeSpeedBuff(null, EventArgs.Empty);
        }

        private void OnStartNegOptimize(object _sender, EventArgs _e)
        {
            if (SetOptimizeSpeedDeBuff != null)
                SetOptimizeSpeedDeBuff(null, EventArgs.Empty);
        }

        private void OnStartGoto(object _sender, EventArgs _e)
        {
            if (SetGotoJumpBuff != null)
                SetGotoJumpBuff(null, EventArgs.Empty);
        }

        private void OnEndDelete(object _sender, EventArgs _e)
        {
            if (DeSetDeleteCollision != null)
                DeSetDeleteCollision(null, EventArgs.Empty);
        }

        private void OnEndPosOptimize(object _sender, EventArgs _e)
        {
            if (DeSetOptimizeSpeedBuff != null)
                DeSetOptimizeSpeedBuff(null, EventArgs.Empty);
        }

        private void OnEndNegOptimize(object _sender, EventArgs _e)
        {
            if (DeSetOptimizeSpeedDeBuff != null)
                DeSetOptimizeSpeedDeBuff(null, EventArgs.Empty);
        }

        private void OnEndGoto(object _sender, EventArgs _e)
        {
            if (DeSetGotoJumpBuff != null)
                DeSetGotoJumpBuff(null, EventArgs.Empty);
        }

        private void OnStartFirewall(object _sender, EventArgs _e)
        {
            if (SetFirewallInvulnBuff != null)
                SetFirewallInvulnBuff(null, EventArgs.Empty);
        }

        private void OnEndFirewall(object _sender, EventArgs _e)
        {
            if (DeSetFirewallInvulnBuff != null)
                DeSetFirewallInvulnBuff(null, EventArgs.Empty);
        }
    }
}
