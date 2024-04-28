using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Chemin du dossier d'entrée et de sortie
        string inputDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "old");
        string outputDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "new");

            // Vérifier si le dossier de sortie existe, sinon le créer
            if (!Directory.Exists(outputDirectoryPath))
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }

            // Obtenir tous les fichiers Lua dans le dossier d'entrée
            string[] inputFiles = Directory.GetFiles(inputDirectoryPath, "*.lua");

            // Expressions régulières pour trouver les entrées de carte avec gather et fight
            Regex gatherRegex = new Regex(@"{map\s*=\s*""([^""]+)"",\s*changeMap\s*=\s*""([^""]+)"",\s*gather\s*=\s*true\s*}");
            Regex fightRegex = new Regex(@"{map\s*=\s*""([^""]+)"",\s*changeMap\s*=\s*""([^""]+)"",\s*fight\s*=\s*true\s*}");

            // Parcourir tous les fichiers d'entrée dans le dossier
            foreach (string inputFilePath in inputFiles)
            {
                // Lire toutes les lignes du fichier d'entrée
                string[] lines = File.ReadAllLines(inputFilePath);

                // Créer une liste pour stocker les lignes modifiées
                List<string> modifiedLines = new List<string>();

                // Parcourir toutes les lignes du fichier d'entrée
                foreach (string line in lines)
                {
                    // Vérifier si la ligne correspond à une entrée de carte avec gather=true
                    if (gatherRegex.IsMatch(line))
                    {
                        // Si la ligne correspond, modifier l'attribut gather
                        string modifiedLine = gatherRegex.Replace(line, "{map = \"$1\", gather = true, changeMap = \"$2\"}");
                        modifiedLines.Add(modifiedLine);
                    }
                    // Vérifier si la ligne correspond à une entrée de carte avec fight=true
                    else if (fightRegex.IsMatch(line))
                    {
                        // Si la ligne correspond, modifier l'attribut fight
                        string modifiedLine = fightRegex.Replace(line, "{map = \"$1\", fight = true, changeMap = \"$2\"}");
                        modifiedLines.Add(modifiedLine);
                    }
                    else
                    {
                        // Si la ligne ne correspond à aucune des conditions ci-dessus, la conserver intacte
                        modifiedLines.Add(line);
                    }
                }

                // Chemin de sortie du fichier
                string outputFilePath = Path.Combine(outputDirectoryPath, Path.GetFileName(inputFilePath));

                // Écrire les lignes modifiées dans le fichier de sortie
                File.WriteAllLines(outputFilePath, modifiedLines);

                Console.WriteLine("Les données modifiées du fichier " + inputFilePath + " ont été enregistrées dans " + outputFilePath);
            }

            Console.WriteLine("Script terminé.");
        }
    }

