//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: src/base.proto
namespace msg
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgHead")]
  public partial class MsgHead : global::ProtoBuf.IExtensible
  {
    public MsgHead() {}
    
    private uint _msgLength;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"msgLength", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public uint msgLength
    {
      get { return _msgLength; }
      set { _msgLength = value; }
    }
    private uint _msgType;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"msgType", DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
    public uint msgType
    {
      get { return _msgType; }
      set { _msgType = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"MsgType")]
    public enum MsgType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SAccount", Value=0)]
      SAccount = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CAccount", Value=1)]
      CAccount = 1
    }
  
}