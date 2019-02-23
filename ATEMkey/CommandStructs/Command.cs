namespace ATEMkey.CommandStructs
{
    public interface ICommand<T>
    {
        int Toggle { get; set; }
        void Execute(T args);
    }
}
