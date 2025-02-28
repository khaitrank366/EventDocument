using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    /*
     *  ========================== EVENT LÀ GÌ??? ==============================
     *  Event về bản chất được xây dựng trên multicast delegate
     *  Được ra đời với mục đích cho phép một đối tượng thông báo 
     *  cho các đối tượng KHÁC khi một hành động cụ thể xảy ra ( được trình bày ở hàm start )
     *  Mang toàn bộ đặt tính của multicast delegate
     *  
     *  ========== TẠI SAO ĐÃ CÓ MULTICAST LẠI CẦN THÊM EVENT ??? ==============
     *  
     *    Đặc điểm                 | Multicast Delegate                                      | Event                                   
     *  |--------------------------|---------------------------------------------------------|----------------------------------------------------------------------------
     *  | Gọi trực tiếp            | ✅ Có thể gọi delegate trực tiếp từ bất kỳ đâu.         | ❌ Chỉ có thể gọi từ bên trong lớp chứa event.                            |
     *  | Tính đóng gói            | ❌ Không bảo vệ, dễ bị gọi sai từ bên ngoài.            | ✅ Bảo vệ tốt hơn, chỉ cho phép đăng ký (+=) và hủy đăng ký (-=).         |
     *  | Tính an toàn             | ❌ Delegate có thể bị gán đè (=), làm mất danh sách gọi | ✅ Event không cho phép gán trực tiếp, chỉ cho phép thêm/xóa phương thức. |
     *  | Khả năng Multicast       | ✅ Hỗ trợ multicast (+=, -=).                           | ✅ Cũng hỗ trợ multicast nhưng an toàn hơn.                               |
     *  | Sử dụng trong thực tế    | 🛠️ Thường dùng cho callback, mẫu thiết kế linh hoạt.    | 🎯 Thường dùng trong lập trình hướng sự kiện.                             |
     *  
     *  Xem ví dụ dưới start để hiểu rõ hơn
     */


    private void Start()
    {
        #region Multicast Delegate

        PlayerMultiCast multiCast = new PlayerMultiCast();
        multiCast.name = "DucTin";

        // Cho phép gán trực tiếp => có thể bị ghi đè các phương thức đã += trước đó
        multiCast.OnChangeName = Client_MulOnChangeName; 
        multiCast.OnChangeName += Server_MulOnChangeName;

        Debug.Log("MultiCast PlayerName: " + multiCast.name);

        // Thay đổi tên player
        multiCast.name = "TinBo";

        // Gọi trực tiếp delegate từ bên ngoài ( không an toàn do có thể bị đổi logic )
        multiCast.OnChangeName?.Invoke("Someone Change Name");

        /*
            MultiCast PlayerName: DucTin
            [Multicast] Change name to: Someone Change Name ( sai logic )
            [Multicast] Update UI: Someone Change Name ( sai logic )
            [Multicast] Logged to server player change name: Someone Change Name ( sai logic )
         */

        #endregion

        #region Event

        Player player = new Player();
        player.playerName = "Linh Xe Om";

        //Chỗ này gán = sẽ báo lỗi Compile => event an toàn hơn multicast Delegate
        //player.OnChangeName = Client_OnChangeName;

        player.OnChangeName += Client_OnChangeName;
        player.OnChangeName += Server_OnChangeName;

        Debug.Log("PlayerName: " + player.playerName);

        // delegate sẽ được lớp Player gọi nội bộ => đảm bảo tính đóng gói => an toàn
        player.playerName = "Born From Ice";

        /*
            PlayerName: Linh Xe Om
            [Event] Change name to: Born From Ice
            [Event] Update UI: Born From Ice
            [Event] Logged to server player change name: Born From Ice
        */

        #endregion

        #region Summarize
        // Event được xây dựng trên MultiCast Delegate
        // nhưng an toàn và đảm bảo tính đóng gói hơn.
        // Không cho phép gán đè (=) và không thể gọi trực tiếp từ bên ngoài lớp.

        /*
         * ========================== LƯU Ý KHI DÙNG EVENT ==============================
         * 
         *  Hủy đăng ký (-=) khi không dùng nữa: Unity không tự động hủy đăng ký sự kiện khi GameObject bị Destroy(), 
         *  dẫn đến memory leak hoặc NullReferenceException nếu sự kiện gọi đến một object đã bị hủy
         *  ==> Luôn hủy đăng ký sự kiện trong OnDestroy() hoặc khi GameObject bị vô hiệu hóa OnDisable().
         *  
         *  Kiểm tra null (?.Invoke()) trước khi gọi sự kiện
         *  Nếu không có subscriber nào đăng ký vào event, event.Invoke() sẽ gây lỗi NullReferenceException
         */
        #endregion

    }

    #region Methods & Classes
    private void Client_OnChangeName(string newName)
    {
        Debug.Log("[Event] Change name to: " + newName);
        Debug.Log("[Event] Update UI: " + newName);
    }

    private void Server_OnChangeName(string newName)
    {
        Debug.Log("[Event] Logged to server player change name: " + newName);
    }

    private void Client_MulOnChangeName(string newName)
    {
        Debug.Log("[Multicast] Change name to: " + newName);
        Debug.Log("[Multicast] Update UI: " + newName);
    }

    private void Server_MulOnChangeName(string newName)
    {
        Debug.Log("[Multicast] Logged to server player change name: " + newName);
    }

    public class Player
    {
        public event Action<string> OnChangeName;
        private string name;

        public string playerName
        {
            get => name;
            set
            {
                name = value;
                OnChangeName?.Invoke(value);
            }
        }
    }

    public class PlayerMultiCast
    {
        // Multicast delegate thay vì event
        public Action<string> OnChangeName;

        public string name;
    }
    #endregion
}


