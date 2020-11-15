using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// 图片相册
    /// </summary>
	public class PictureAlbum : BaseBLL<PictureAlbumInfo>
    {
        public PictureAlbum() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
