using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class UserForCreationDto
    {
        public string UserName { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        public string HinhAnh { get; set; }
    }

    public class UserForUpdateDto
    {
        public string UserName { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        public string HinhAnh { get; set; }
    }
}
