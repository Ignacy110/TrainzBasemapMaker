// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainzBasemapMaker.Classes
{
    public class UpdateInfo
    {
        public bool IsNewerAvailable { get; set; }
        public string LatestVersionTag { get; set; } = string.Empty;
        public string HtmlUrl { get; set; } = string.Empty;
    }

    internal class GitHubReleaseDto
    {
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;

        [JsonPropertyName("draft")]
        public bool Draft { get; set; }

        [JsonPropertyName("prerelease")]
        public bool Prerelease { get; set; }
    }

    public static class UpdateChecker
    {
        private static readonly HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(6)
        };

        private const string ReleasesApiUrl = "https://api.github.com/repos/Ignacy110/TrainzBasemapMaker/releases";

        public static async Task<UpdateInfo?> CheckForUpdateAsync(string currentVersionTag)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, ReleasesApiUrl);
                request.Headers.Add("User-Agent", "TrainzBasemapMaker-UpdateChecker");

                using var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var releases = JsonSerializer.Deserialize<GitHubReleaseDto[]>(json);

                if (releases == null || releases.Length == 0)
                    return null;

                // Znajdź najnowsze wydanie, które nie jest szkicem (draft)
                GitHubReleaseDto? latestRelease = null;
                foreach (var rel in releases)
                {
                    if (!rel.Draft)
                    {
                        latestRelease = rel;
                        break;
                    }
                }

                if (latestRelease == null)
                    return null;

                bool isNewer = IsRemoteVersionNewer(currentVersionTag, latestRelease.TagName);

                return new UpdateInfo
                {
                    IsNewerAvailable = isNewer,
                    LatestVersionTag = latestRelease.TagName,
                    HtmlUrl = latestRelease.HtmlUrl
                };
            }
            catch
            {
                // Wszelkie błędy sieciowe/timeouty po cichu ignorujemy
                return null;
            }
        }

        public static bool IsRemoteVersionNewer(string currentTag, string remoteTag)
        {
            if (string.Equals(currentTag.Trim(), remoteTag.Trim(), StringComparison.OrdinalIgnoreCase))
                return false;

            var (curVer, curPre) = ParseVersionTag(currentTag);
            var (remVer, remPre) = ParseVersionTag(remoteTag);

            if (curVer != null && remVer != null)
            {
                if (remVer > curVer) return true;
                if (remVer < curVer) return false;

                // Wersje numeryczne są identyczne (np. 0.6.0 vs 0.6.0-alpha).
                // Zgodnie z SemVer wersja bez oznaczenia pre-release (stabilna) jest nowsza niż z pre-release.
                bool curHasPre = !string.IsNullOrEmpty(curPre);
                bool remHasPre = !string.IsNullOrEmpty(remPre);

                if (curHasPre && !remHasPre) return true;  // Zdalna to wydanie stabilne, lokalna to pre-release
                if (!curHasPre && remHasPre) return false; // Lokalna to wydanie stabilne, zdalna to pre-release

                if (curHasPre && remHasPre)
                {
                    return ComparePreRelease(curPre, remPre) < 0;
                }

                return false;
            }

            return false;
        }

        private static (Version? Version, string PreRelease) ParseVersionTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return (null, string.Empty);

            string clean = tag.Trim().TrimStart('v', 'V');
            int dashIndex = clean.IndexOf('-');
            string versionPart = dashIndex >= 0 ? clean.Substring(0, dashIndex) : clean;
            string prePart = dashIndex >= 0 ? clean.Substring(dashIndex + 1) : string.Empty;

            if (!versionPart.Contains('.'))
            {
                versionPart += ".0";
            }

            if (Version.TryParse(versionPart, out var version))
            {
                return (version, prePart);
            }

            return (null, prePart);
        }

        private static int ComparePreRelease(string curPre, string remPre)
        {
            int curRank = GetPreReleaseRank(curPre);
            int remRank = GetPreReleaseRank(remPre);

            if (curRank != remRank)
            {
                return curRank.CompareTo(remRank);
            }

            return StringComparer.OrdinalIgnoreCase.Compare(curPre, remPre);
        }

        private static int GetPreReleaseRank(string pre)
        {
            string lower = pre.ToLowerInvariant();
            if (lower.StartsWith("alpha")) return 1;
            if (lower.StartsWith("beta")) return 2;
            if (lower.StartsWith("rc")) return 3;
            return 0;
        }
    }
}
