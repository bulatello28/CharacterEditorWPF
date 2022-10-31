using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditorCore
{
    public class Match
    {
        public ObjectId matchId { get; set; }

        public List<ObjectId> whiteTeam;
        public List<ObjectId> blackTeam;

        public Match()
        {
            whiteTeam = new List<ObjectId>();
            blackTeam = new List<ObjectId>();
        }
    }
}
