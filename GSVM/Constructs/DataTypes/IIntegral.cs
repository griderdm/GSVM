namespace GSVM.Constructs.DataTypes
{
    public interface IIntegral : IDataType
    {
        object Value { get; }
        TOut CastTo<TOut>() where TOut : IIntegral;
        IIntegral CastTo(IIntegral tout);
    }

    public interface IIntegral<T> : IIntegral
    {
        new T Value { get; set; }
    }
}
