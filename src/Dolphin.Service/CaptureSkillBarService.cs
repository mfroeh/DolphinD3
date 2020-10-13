namespace Dolphin.Service
{
    public class CaptureSkillBarService : ICaptureScreenService
    {
        public object Capture()
        {
            var handle = WindowInformationHelper.GetHWND("Diablo 3");
            // Capture logic
            return null;
        }
    }
}