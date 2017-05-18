
namespace skntcp
{
    public interface IService
    {
        void setHandler(IHandler handler);
        ProtocolCodecFilter  getProtocalCodecFilter();
        void setProtocolCodecFilter(ProtocolCodecFilter f);
    }
}
