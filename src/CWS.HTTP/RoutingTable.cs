namespace CWS.HTTP
{
    using System;
    using System.Collections.Generic;

    public class RoutingTable
    {
        private readonly Dictionary<Route, Action<HttpRequest, HttpResponse>> routes = new();

        private readonly bool overrideExistingRoutes;

        public RoutingTable(bool overrideExistingRoutes = true)
        {
            this.overrideExistingRoutes = overrideExistingRoutes;
        }

        public void AddRoute(Method method, string path, Action<HttpRequest, HttpResponse> action)
        {
            var route = new Route(method, path);

            if (!routes.ContainsKey(route))
            {
                routes.Add(route, action);
                return;
            }

            if (overrideExistingRoutes)
            {
                routes[route] = action;
            }
            else
            {
                throw new ArgumentException($"{nameof(route)} already exists");
            }
        }

        public Action<HttpRequest, HttpResponse> GetAction(Route route)
        {
            if (!routes.ContainsKey(route))
            {
                return null;
            }

            return routes[route];
        }
    }
}
