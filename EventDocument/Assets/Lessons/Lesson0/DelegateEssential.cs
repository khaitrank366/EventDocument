using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class DelegateEssential : MonoBehaviour
{
    /*
     * ======================== DELEGATE LÀ GÌ ??? ================================
     * Delegate là 1 BIẾN kiểu tham chiếu chứa tham chiếu tới 1 hoặc nhiều hàm
     * Chúng ta có thể hiểu tương tự con trỏ hàm
     * Vì là một biến nên giá trị của Delegate có thể thay đổi lúc RunTime ( trình bày ở hàm Start )
     * Hiểu đơn giản thì Delegate như 1 biến thay thế cho hàm
     */

    /*
     * ======================== TẠI SAO CẦN DÙNG DELEGATE ??? ======================
     * Khi cần dùng 1 hàm như một biến
     * vd: tham số truyền vào là 1 ham như callback ( trình bày rõ hơn ở DelegateExtended) hoặc là event ( trình bày rõ hơn ở DelegateEvent)    
     * Dùng tốt với các hàm ẩn danh (anonymous methods) hoặc biểu thức lambda (lambda expressions) ( Trình bày rõ hơn ở DelegateExtended )
     */

    // ======================== CÁCH KHAI BÁO DELEGATE =============================

    /* 
       Phải khai báo ở cấp độ class
       Phải cùng KIỂU DỮ LIỆU TRẢ VỀ và THAM SỐ ĐẦU VAÒ với hàm muốn trỏ tới
       Cú pháp: [delegate] [kiểu trả về] [tên delegate] ([Danh sách tham số đầu vào])
    */

    // Khai báo delegate với kiểu trả về là void và tham số là một chuỗi (string)
    public delegate void MyDelegate(string message);

    public delegate void MulticastDelegate(string message);

    private void Start()
    {
        // Tạo instance của delegate và gán phương thức ( giống gán giá trị cho biến )
        MyDelegate myDelegate = ShowMessage;

        // Gọi phương thức thông qua delegate
        myDelegate("Hello, Delegate!");

        // Gán phương thức khác cho delegate 
        myDelegate = PrintMessage;
        myDelegate("Another message using delegate.");


        /*
            ShowMessage: Hello, Delegate!
            PrintMessage: Another message using delegate.
        */


        /*
         * ========================== MULTICAST DELEGATE ==============================
         * Đặt biệt, khác với biến, Delegate có thể được gán nhiều giá trị cùng 1 lúc bằng toán từ cộng (+=)
         * Giúp Code ngắn gọn hơn, tăng tính linh hoạt hơn
         * Áp dụng tốt trong lập trình hướng sự kiện
         * vd: thực hiện các tác vụ đồng loạt như: ghi log, gửi thông báo, cập nhật giao diện,...
         */

        MulticastDelegate notify = SendEmail;
        notify += LogToFile;
        notify += ShowNotification;

        // Gọi multicast delegate
        Debug.Log("=== Gọi tất cả phương thức ===");
        // C1: gọi thẳng delegate ( sẽ báo lỗi nếu chưa gán hàm cho delegate )
        notify("Multicast delegate example!");
        // C2: Invoke ( không báo lỗi những sẽ bị lỗi RunTime nếu chưa gán hàm )
        notify?.Invoke("Multicast delegate example!");

        // Hủy đăng ký phương thức khỏi delegate
        notify -= LogToFile;
        Debug.Log("\n=== Sau khi hủy LogToFile ===");
        notify("Delegate after unsubscription.");
        notify?.Invoke("Delegate after unsubscription.");

        /*
                 === Gọi tất cả phương thức ===
            📧 Email sent: Multicast delegate example!
            📝 Log saved: Multicast delegate example!
            🔔 Notification: Multicast delegate example!

                === Sau khi hủy LogToFile ===
            📧 Email sent: Delegate after unsubscription.
            🔔 Notification: Delegate after unsubscription.

         */

        /*
         * ================ LƯU Ý QUAN TRỌNG KHI DÙNG MULTICAST DELEGATE ==============
         * Nếu delegate có giá trị trả về, chỉ giá trị của phương thức cuối cùng trong danh sách gọi mới được trả về
         * Nếu không xử lý kỹ các exeption hoặc lỗi trong các hàm được gán (+=) , các phương thức tiếp theo sẽ không thể được thực hiện
         */
    }

    #region simple delegate
    private void ShowMessage(string message)
    {
        Debug.Log("ShowMessage: " + message);
    }

    private void PrintMessage(string message)
    {
        Debug.Log("PrintMessage: " + message);
    }
    #endregion

    #region Multicast Delegate
    // Các phương thức xử lý khác nhau
    public static void SendEmail(string message)
    {
        Debug.Log("📧 Email sent: " + message);
    }

    public static void LogToFile(string message)
    {
        Debug.Log("📝 Log saved: " + message);
    }

    public static void ShowNotification(string message)
    {
        Debug.Log("🔔 Notification: " + message);
    }
    #endregion 
}
