// This file is a part of ApiHub project.
// ApiHub belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Runtime.CompilerServices;

namespace ApiHub.Shared.Extensions;

public static class NullableCheckExtension
{
    public static T NotNull<T>(this T? value, [CallerMemberName] string name = "?? Unknown ??")
    {
        if (value is null)
        {
            throw new ArgumentNullException(name);
        }

        return value;
    }
}
