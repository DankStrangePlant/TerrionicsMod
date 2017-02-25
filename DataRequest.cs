namespace TerrionicsMod
{
    class DataRequest
    {
        public string route;
        public ulong interval;

        public DataRequest(string route="/", ulong interval=1)
        {
            this.route = route;
            this.interval = interval;
        }
    }
}
