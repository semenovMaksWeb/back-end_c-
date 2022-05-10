namespace back_end.Types.Screen
{
    public class TypeScreenApi
    {
        public TypeScreen screen { get; set; }
        public List<TypeBreadcrumbscs> breadcrumbs{ get; set;}

        public Dictionary<string,TypeComponents> components { get; set; }
    }
}
