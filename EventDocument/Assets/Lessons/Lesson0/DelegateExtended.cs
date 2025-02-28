using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateExpand : MonoBehaviour
{
    /*
     * ========================== MỘT SỐ LOẠI DELEGATE TÍCH HỢP SẴN ==============================
     * =================================( BUILT-IN DELEGATE )=====================================
     * 
     * Thay vì phải khai báo theo chuẩn cú pháp delegate thì code sẽ khá dài
     * Đó là lí do c# định nghĩa sẵn 1 vài build in để tránh boilerplate code
     * 
     * Boilerplate code là những đoạn mã lặp lại, mang tính khuôn mẫu, không tạo ra 
     * nhiều giá trị thực sự mà chỉ nhằm đáp ứng yêu cầu cú pháp hoặc thiết lập cấu trúc cơ bản
     * 
     * Action<T>: Dùng cho các phương thức không trả về giá trị (void).
     * Func<T, TResult>: Dùng cho các phương thức có giá trị trả về (TResult).
     * Predicate<T>: Trả về bool, thường dùng trong các phép kiểm tra điều kiện.
     * 
     * Chổ <T> sẽ là nơi cho các tham số đầu vào
     * 
     * Mở region Build-in Delegates để đọc demo code ( đọc xong nhớ đóng lại để tránh đi cảnh nhé xD )
     */

    /*
     * ==================================== CALLBACK =============================================
     * Delegate về bản chất là BIẾN => có thể được truyền vào hàm như parameter 
     * => CallBack cơ bản là 1 delegate được truyền vào hàm để thực hiện 
     * vào một thời điểm nhất định, chẳng hạn sau khi hoàn thành một tác vụ nào đó.
     * 
     * Lợi ích:
     * Tính linh hoạt cao: Chỉ cần thay đổi callback, không cần sửa đổi code
     * Dễ chia tách module: Code dễ dàng mở rộng và bảo trì.
     * Né được hard-code ( trải nghiệm cá nhân ): logic dựa theo hàm được gán chứ không hard-code để xử lý      
     */

    void Start()
    {
        #region Build-in Delegates

        // Action delegate (không có giá trị trả về): Dev Unity sẽ thấy rất quen :v
        Action<string> greet = name => Debug.Log("Hello, " + name);
        greet("Born From The Rock");

        // Action delegate hỗ trợ từ 0 - 16 tham số đầu vào
        Action<int, int, int, int, int> add = (a, b, c, d, e) => Debug.Log("Action: " + a + b + c + d + e);
        add(3, 5, 7, 11, 9);

        // Khai báo delegate truyền thống sẽ trông như thế này ( đã kết hợp lamda expression, nếu không kết hợp phải khai báo thêm 1 hàm )
        // delegate void ActionDelegate(int a, int b);
        // ActionDelegate actionDelegate = (a, b) => Debug.Log(a + b);

        // Func delegate (có giá trị trả về): param cuối cùng sẽ là kiểu trả về
        Func<int, int, int> sum = (a, b) => a + b;
        int result = sum(10, 20);
        Debug.Log("Sum: " + result);

        // Khai báo delegate truyền thống sẽ trông như thế này
        // delegate int FuncDelegate(int a, int b);
        // FuncDelegate funcDelegate = (a, b) => a + b;

        // Predicate delegate (trả về true/false)
        Predicate<int> isEven = number => number % 2 == 0;
        bool boolResult = isEven(5);
        Debug.Log("Is 5 even? " + boolResult);

        // Khai báo delegate truyền thống sẽ trông như thế này
        // delegate bool PredicateDelegate(int number);
        // PredicateDelegate preDelegate = (number) => number % 2 == 0;

        #endregion
    }

    #region Callback
    private void Update()
    {
        // Chọn callback
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChooseCallback("1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChooseCallback("2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChooseCallback("3");
        }
    }

    public DataLoader dataLoader;
    public CallbackHandler callbackHandler;
    private void ChooseCallback(string choice)
    {
        Action<string> callback = null;
        switch (choice)
        {
            case "1":
                callback = callbackHandler.PrintToConsole;
                break;
            case "2":
                callback = callbackHandler.SaveToFile;
                break;
            case "3":
                callback = callbackHandler.SendToServer;
                break;
            default:
                callback = callbackHandler.PrintToConsole;
                break;
        }

        dataLoader.LoadData(callback);
    }
    #endregion
}

public class DataLoader : MonoBehaviour
{
    // Phương thức tải dữ liệu và gọi callback khi hoàn tất
    public void LoadData(Action<string> callback)
    {
        Debug.Log("Bắt đầu tải dữ liệu...");
        StartCoroutine(SimulateDataLoading(callback));
    }

    private IEnumerator SimulateDataLoading(Action<string> callback)
    {
        yield return new WaitForSeconds(2f); // Giả lập tải dữ liệu trong 2 giây
        string data = "Dữ liệu tải xong từ server Unity";

        // Gọi hàm callback và truyền dữ liệu
        callback?.Invoke(data);
    }
}

public class CallbackHandler
{
    public void PrintToConsole(string data)
    {
        Debug.Log($"[Console] Dữ liệu nhận được: {data}");
    }

    public void SaveToFile(string data)
    {
        Debug.Log($"[File] Dữ liệu đã được lưu: " + data);
    }

    public void SendToServer(string data)
    {
        Debug.Log($"[Server] Dữ liệu '{data}' đã được gửi tới server.");
    }

}
