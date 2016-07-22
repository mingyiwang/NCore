using System;
using System.Globalization;

namespace Core.Service {

    public sealed class ServiceState {

        /// <summary>
        /// A service in this state is inactive.
        /// </summary>
        public const int Created   = 100;

        /// <summary>
        /// A service in this state is transitioning to Started State
        /// </summary>
        public const int Starting  = 200;

        /// <summary>
        /// A service in this state is operational and all resources all configured
        /// </summary>
        public const int Resuming  = 202;

        /// <summary>
        /// A service in this state is operational and all resources all configured
        /// </summary>
        public const int Started   = 201;

        /// <summary>
        /// A service in this state is transitioning to Stopped State
        /// </summary>
        public const int Stopping  = 400;

        /// <summary>
        /// A service in this state is not operational but can be restarted
        /// </summary>
        public const int Stopped   = 401;

        /// <summary>
        /// A service in this state is will eligible for garbage collection
        /// </summary>
        public const int Destroyed = 500;

        /// <summary>
        /// A service in this state is not operational and can not be restarted, stopped and termiated
        /// </summary>
        public const int Failed    = 444;

        public string Name {
            get; private set;
        }

        public int Code {
            get; private set;
        }

        public bool CanStopOnStarting {
            get; private set;
        }

        public ServiceState() : this(Created) {}

        public ServiceState(int code) {
            SetState(code);
        }

        public void SetState(int code) {
            SetState(code, false);
        }

        public void SetState(int code, bool stopWhenStarting) {
            CanStopOnStarting = stopWhenStarting;
            Code = code;
            Name = GetName(code);
        }

        public static string GetName(int code) {
            switch(code) {
                case Created     : return "Created";
                case Starting    : return "Starting";
                case Started     : return "Started";
                case Stopping    : return "Stopping";
                case Stopped     : return "Stopped";
                case Failed      : return "Failed";
                default          : throw new ArgumentException("Unknow State Code#" + code);
            }
        }

        public static int GetCode(string name) {
            var lcName = name.ToLower(CultureInfo.InvariantCulture);
            switch (lcName) {
                case "initialized" : return Created;
                case "starting"    : return Starting;
                case "started"     : return Started;
                case "stopping"    : return Stopping;
                case "stopped"     : return Stopped;
                case "failed"      : return Failed;
                default            : throw new ArgumentException("Unknow State#" + name);
            }
        }

    }

}