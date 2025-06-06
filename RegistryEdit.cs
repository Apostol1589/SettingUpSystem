namespace SettingUpSystem
{
    internal class RegistryEdit
    {
        public string Path { get; }
        public string Name { get; }
        public object Value { get; }

        public RegistryEdit(string path, string name, object value)
        {
            Path = path;
            Name = name;
            Value = value;
        }
    }
}
