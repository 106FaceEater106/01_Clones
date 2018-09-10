using System;
using System.Collections.Generic;
using System.Text;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private class Program
        {
            public Program(string name, Program prev) { Name = name; Prev = prev; }
            public Program Prev { get; set; }
            public string Name { get; set; }
        }

        private class Clone
        {
            Program learned;
            Program undoes;
            public Clone() { }
            public Clone(Clone other)
            {
                learned = other.learned;
                undoes = other.undoes;
            }

            public void Learn(string program)
            {
                undoes = null;
                learned = new Program(program, learned);
            }

            public void Rollback()
            {
                undoes = new Program(learned.Name, undoes);
                learned = learned.Prev;
            }

            public void Relearn()
            {
                learned = new Program(undoes.Name, learned);
                undoes = undoes.Prev;
            }

            public string GetCurrentProgram()
            {
                return null == learned ? "basic" : learned.Name;
            }
        }

        private int nextId = 1;
        private Dictionary<int, Clone> clones = new Dictionary<int, Clone>();

        public CloneVersionSystem()
        {
            clones[nextId++] = new Clone();
        }

        public string Execute(string query)
        {
            var v = query.Split(' ');
            var op = v[0];
            var id = int.Parse(v[1]);
            if (op == "learn") clones[id].Learn(v[2]);
            else if (op == "rollback") clones[id].Rollback();
            else if (op == "relearn") clones[id].Relearn();
            else if (op == "clone") clones[nextId++] = new Clone(clones[id]);
            else if (op == "clone") clones[nextId++] = new Clone(clones[id]);
            else if (op == "check") return clones[id].GetCurrentProgram();
            else throw new ArgumentException("unknown operation: " + op);
            return null; // for all except Check
        }
    }
}
