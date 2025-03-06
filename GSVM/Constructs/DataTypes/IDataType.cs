namespace GSVM.Constructs.DataTypes
{
    public interface IDataType
    {
        SmartPointer Pointer { get; }
        uint Address { get; set; }
        uint Length { get; }
        byte[] ToBinary();
        void FromBinary(byte[] value);
    }
}
