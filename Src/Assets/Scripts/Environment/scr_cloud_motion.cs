using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace environment
{
    public class scr_cloud_motion : MonoBehaviour
    {
        private float m_cloud_speed = 0.0f;
        private float m_direction = 0.0f; // 1 = right, -1 = left 
        private float m_start_x = 0.0f;
        private float m_start_y = 0.0f;

        private float m_alpha = 1.0f;

        private float m_cloud_scale =1.0f;

        private const string FILE_NAME_CLOUD = "Prefab/obj_simple_cloud";
        private GameObject m_cloud_obj;

        public scr_cloud_motion(float cloud_speed, float direction, float start_x, float start_y, float alpha, float scale)
        {
            m_cloud_speed = cloud_speed;
            m_direction = direction;
            m_start_x = start_x;
            m_start_y = start_y;
            m_alpha = alpha;
            m_cloud_scale = scale;

            string path = Directory.GetCurrentDirectory();

            init_cloud();
        }
        private void init_cloud()
        {
            GameObject obj_type = Resources.Load(FILE_NAME_CLOUD) as GameObject;

            if (obj_type == null)
            {
                throw new FileNotFoundException("Could not find cloud game object. \n");
            }

            Vector2 start_pos = new Vector2(m_start_x, m_start_y);

            m_cloud_obj = Instantiate(obj_type, start_pos, Quaternion.identity);

            var tmp = m_cloud_obj.GetComponent<SpriteRenderer>();
            Color new_clr = m_cloud_obj.GetComponent<SpriteRenderer>().color;

            new_clr.a = m_alpha; 
            tmp.color = new_clr;
            tmp.drawMode = SpriteDrawMode.Sliced;
            tmp.size += new Vector2(Mathf.Abs(1 - m_cloud_scale), Mathf.Abs(1 - m_cloud_scale) * 0.5f);
            var e = m_cloud_obj.GetComponent<SpriteRenderer>().size;
            Debug.Log(e);
        }
        void Update()
        {
            move_cloud();
        }
        public void move_cloud()
        {
            Vector3 current_pos = m_cloud_obj.transform.position;
            current_pos.x += m_cloud_speed * m_direction;
            m_cloud_obj.transform.position = current_pos;
        }
    }

}
