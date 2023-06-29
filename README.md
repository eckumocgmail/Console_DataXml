    public string UpdateDatabase(string context, string name)
    {
        ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C " +
                    @$"cd {_wrk} && dotnet ef database update --context {context} --no-build");

        info.RedirectStandardError = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
        return process.StandardOutput.ReadToEnd();
    }
