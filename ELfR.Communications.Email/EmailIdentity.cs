namespace ELfR.Communications.Email
{
    /// <summary>
    /// An e-mail identity (sender or recipient).
    /// </summary>
    /// <param name="Address">The e-mail address.</param>
    /// <param name="Name">The name.</param>
    public record EmailIdentity(string Address = default!, string Name = default!);
}