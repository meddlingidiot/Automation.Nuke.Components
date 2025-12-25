using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace Automation.Nuke.Components.Parameters;

public interface IHasSolution : INukeBuild
{
    [Solution]
    Solution Solution => TryGetValue(() => Solution);
}