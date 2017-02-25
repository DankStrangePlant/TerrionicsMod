using System;

namespace TerrionicsMod
{
    class ActionRequest
    {
        public string route;
        public Action action;

        public ActionRequest(string route="/")
        {
            this.route = route;
        }
    }
}
