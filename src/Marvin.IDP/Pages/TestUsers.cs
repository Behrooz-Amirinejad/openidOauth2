// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace Marvin.IDP;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = "69118",
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "26AEBC36-FBE7-48FC-B0ED-C96729FEE238",
                    Username = "ali",
                    Password = "ali",
                    Claims = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.GivenName , "Ali"),
                        new Claim(JwtClaimTypes.FamilyName , "Alireza")
                    }
                },
                new TestUser
                {
                    SubjectId = "122F2F50-FBC1-4369-9A03-A618FFE51D1A",
                    Username = "ben",
                    Password = "ben",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.GivenName , "Ben"),
                        new Claim(JwtClaimTypes.FamilyName , "Benza")
                    }
                }
            };
        }
    }
}