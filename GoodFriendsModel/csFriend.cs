﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodFriendsModel
{
    public class csFriend
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public csAdress Adress { get; set; } = null;    //null = no adress        

        public List<csPet> Pets { get; set; } = null;      //null = no pets 


        public string FullName => $"{FirstName} {LastName}";
        public override string ToString()
        {
            var sRet = FullName;

            if (Adress != null)
            {
                sRet += $". lives at {Adress}";
            }

            if (Pets != null)
            {
                sRet += $". Has pets ";
                foreach (var pet in Pets)
                {
                    sRet += $"{pet}, ";
                }
            }
            return sRet;
        }

        public static class Factory
        {
            public static csFriend CreateRandom()
            {
                var rnd = new csRandomData();

                var fn = rnd.FirstName;
                var ln = rnd.LastName;
                var country = rnd.Country;

                //Create between 0 and 3 pets
                var _pets = new List<csPet>();
                for (int i = 0; i < rnd.Next(0,4); i++)
                {
                    _pets.Add(csPet.Factory.CreateRandom()); 
                }

                return new csFriend
                {
                    FirstName = fn,
                    LastName = ln,
                    Email = rnd.Email(fn, ln),
                    Adress = (rnd.Bool) ?csAdress.Factory.CreateRandom() :null,
                    Pets = (_pets.Count > 0) ? _pets : null  
                };
            }
        }
    }
}

