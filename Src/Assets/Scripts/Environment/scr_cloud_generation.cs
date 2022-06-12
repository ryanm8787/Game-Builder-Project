using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace environment
{
    public class scr_cloud_generation : MonoBehaviour
    {
        public float cloud_speed = 0;
        public int min_frame_interval = 0;
        public int max_frame_interval = 0;

        public float min_y_start = 0.0f;
        public float max_y_start = 0.0f;

        private const int CUT_OFF_LEFT = 50; // anything < this will mean the direction is negative
        private const int PROB_MAX = 100;

        private int m_fps = -1;
        private int m_fps_counter = 0;

        private List<scr_cloud_motion> m_clouds;
        private float m_x_start = 0;
        private const float X_CONST = 10f;

        private const float CLOUD_ALPHA_MIN = 0.5f;
        private const float CLOUD_ALPHA_MAX = 0.75f;
        private void Start()
        {
            m_clouds = new List<scr_cloud_motion>();
            if (min_frame_interval > max_frame_interval)
            {
                throw new InvalidDataException("Minimum frame interval cannot be greater than max");
            }
        }
        private int determine_direction(int val)
        {
            if(val < CUT_OFF_LEFT)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        private int generate_interval()
        {
            // TODO single instance please x
            System.Random cloud_interval_generation = new System.Random();

            return cloud_interval_generation.Next(min_frame_interval, max_frame_interval);
        }

        private float generate_position_y()
        {
            // TODO single instance please x
            System.Random cloud_interval_generation = new System.Random();

            // TODO: this is so shit me
            return (float)cloud_interval_generation.NextDouble() * (max_y_start - min_y_start) + min_y_start;
        }
        
        // TODO implement
        private float generate_img_scale()
        {
            // TODO single instance please x
            System.Random cloud_interval_generation = new System.Random();
            int scale_upper = 800;
            int scale_lower = 100;            

            float scale = cloud_interval_generation.Next(scale_lower, scale_upper) / scale_lower; 
            return scale;
        }
        
        private float generate_cloud_speed()
        {
            // TODO single instance please x
            System.Random speed_generator = new System.Random();
            float cloud_speed_min = 0.2f * cloud_speed;

            float cloud_speed_new = (float)speed_generator.NextDouble() * (cloud_speed - cloud_speed_min) + cloud_speed_min;

            return cloud_speed_new;
        }

        private float generate_cloud_alpha()
        {
            // TODO single instance please x
            System.Random alpha_generator = new System.Random();

            float cloud_alpha_new = (float)alpha_generator.NextDouble() * (CLOUD_ALPHA_MAX - CLOUD_ALPHA_MIN) + CLOUD_ALPHA_MIN;

            return cloud_alpha_new;
        }

        private void generate_cloud()
        {
            // these are usually shit generators but will work for our case 
            System.Random cloud_dir_generator = new System.Random();
            int cloud_dir_raw = cloud_dir_generator.Next(0, PROB_MAX);

            int cloud_dir = determine_direction(cloud_dir_raw);
            float start_y = generate_position_y();

            m_x_start = X_CONST * cloud_dir * -1;

            // TODO: update co-ordinates.
            scr_cloud_motion new_cloud = new scr_cloud_motion(generate_cloud_speed(), cloud_dir, m_x_start, start_y, generate_cloud_alpha(), generate_img_scale());
            m_clouds.Add(new_cloud);
        }

        // Update is called once per frame
        void Update()
        {
            if(m_fps == -1)
            {
                // TODO: this might not work
                m_fps = (int)(1 / Time.deltaTime);
            }

            if(m_fps_counter < 1)
            {
                m_fps_counter = generate_interval();
                generate_cloud();
            }
            else
            {
                m_fps_counter--;
            }

            foreach (scr_cloud_motion cloud in m_clouds)
            {
                // check if cloud is dead and remove from list. If we're removing maybe use standard int i for loop

                // move cloud
                cloud.move_cloud();
            }
        }
    }
}