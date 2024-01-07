using Code.Runtime.Logic;
using Code.Runtime.Logic.Interactions;
using Code.Runtime.Logic.Interactions.Data;

namespace Code.Runtime.Services.Interactions.ScannerInteraction
{
    internal interface IScannerInteractionService
    {
        void Interact(BookStorage bookStorage, Progress progress, Scanner scanner);
        void StartScanningIfPossible(BookStorage bookStorage, Progress progress, Scanner scanner);
        void StopReading(Progress progress);
        void CanInteract(IBookStorage bookStorage, IProgress progress);
    }
}