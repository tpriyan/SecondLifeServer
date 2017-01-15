﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TextureChangerConstants
/// </summary>
/// 
namespace TextureChanger
{
    public class Constants
    {
        public static string QueryReadAll = "select* from InworldObjects";
        public static string QueryReadFilterObjectId = "select * from InworldObjects where ObjectGUID = '{0}'";
        public static string QueryInsert = "insert into InworldObjects(ObjectGUID, URL, Name, Type, Owner) values ('{0}', '{1}', '{2}', '{3}', '{4}')";
        public static string QueryUpdate = "update InworldObjects set URL = '{0}', Name='{1}', Type = '{2}', Owner = '{3}'  where ObjectGUID = '{4}'";


        public Constants()
        {
        }
    }
}