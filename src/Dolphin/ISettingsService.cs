namespace Dolphin
{
    public interface ISettingsService
    {
        Settings Settings { get; }

        UISettings UISettings { get; }
    }
}
