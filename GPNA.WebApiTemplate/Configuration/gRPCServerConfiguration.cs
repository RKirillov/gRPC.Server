namespace GPNA.gRPCServer.Configuration
{
    public class gRPCServerConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public int BatchCount { get; private set; } = 10000;
        /// <summary>
        /// 
        /// </summary>
        public int HelthCheckPeriod { get; private set; } = 30;
        /// <summary>
        /// 
        /// </summary>
        public int HelthCheckDelay { get; private set; } = 30;
        /// <summary>
        /// 
        /// </summary>
        public bool EnableDetailedErrors { get; private set; } = true;
    }
}
