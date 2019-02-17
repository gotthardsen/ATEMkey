namespace ATEMkey.CommandStructs
{
    public interface ICommand<T>
    {
        void Execute(T args);
    }
}
