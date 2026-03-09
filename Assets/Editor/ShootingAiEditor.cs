using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShootingAI))]
public class AIShootingEditor : Editor
{
    private void OnSceneGUI()
    {
        // Lấy tham chiếu đến script AI đang được chọn
        ShootingAI ai = (ShootingAI)target;

        // Xác định tâm của vùng tuần tra (startPosition khi đang chơi, hoặc transform.position khi thiết kế)
        // Lưu ý: ai.startPosition phải là biến public hoặc được truy cập được từ đây
        Vector2 center = Application.isPlaying ? (Vector2)ai.transform.position : (Vector2)ai.transform.position;

        Handles.color = Color.green;

        if (ai.patrolPoints == null) return;

        for (int i = 0; i < ai.patrolPoints.Length; i++)
        {
            // Chuyển sang tọa độ World để hiển thị trên màn hình Scene
            Vector2 worldPoint = center + ai.patrolPoints[i];
            
            EditorGUI.BeginChangeCheck();
            
            // Tạo nút kéo hình tròn (DotHandle) tại mỗi điểm
            var fmh_28_72_639085853554699084 = Quaternion.identity; Vector2 newWorldPoint = Handles.FreeMoveHandle(worldPoint, 0.2f, Vector3.zero, Handles.DotHandleCap);
            
            if (EditorGUI.EndChangeCheck())
            {
                // Cho phép nhấn Ctrl+Z để quay lại nếu kéo nhầm
                Undo.RecordObject(ai, "Edit Patrol Points");
                
                // Cập nhật lại tọa độ Local vào mảng patrolPoints
                ai.patrolPoints[i] = newWorldPoint - center;
                
                // Báo cho Unity biết dữ liệu đã thay đổi để lưu lại
                EditorUtility.SetDirty(ai);
            }

            // Vẽ số thứ tự điểm để bạn dễ theo dõi điểm nào nối với điểm nào
            Handles.Label(worldPoint + Vector2.up * 0.3f, "Point " + i);
        }
    }
}
