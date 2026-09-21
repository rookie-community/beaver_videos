using System.Buffers.Binary;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Beaver.Data
{
    /// <summary>
    /// 内置账号的种子常量。
    /// 这些值由 <see cref="Configurations.UserConfiguration"/> 的 HasData 引用，
    /// 在 <c>EnsureCreated</c> 建库时随表结构一并写入，不再有任何运行时写库逻辑。
    /// 因此它们必须是**确定值**：同一个模型（含种子）在任意机器、任意次建模下都完全一致。
    /// </summary>
    public static class AppSeedData
    {
        /// <summary>内置管理员登录名。开发环境下登录页会用它预填表单。</summary>
        public const string DefaultUserName = "admin";

        /// <summary>内置管理员初始口令。明文只用于生成下面的哈希和开发环境预填表单，不入库。</summary>
        public const string DefaultPassword = "123456";

        /// <summary>内置管理员固定主键，便于多次重置数据库时得到同一行记录。</summary>
        public static readonly Guid AdministratorUserId = Guid.Parse("b1f4c6a2-7d31-4c58-9e2a-6f0d81c3a001");

        /// <summary>种子行的创建时间（固定 UTC 值，保证模型种子确定）。</summary>
        public static readonly DateTime AdministratorCreationTime = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // 固定盐。它是公开的默认口令，用固定盐不影响实际安全性，
        // 但能让同一口令每次都得到同一个哈希，从而可以作为 HasData 的确定值。
        private static readonly byte[] SeedSalt = Encoding.UTF8.GetBytes("BeaverVideos.Seed.Salt");

        // 与 Microsoft.AspNetCore.Identity.PasswordHasher 的默认参数保持一致。
        private const int IterationCount = 100_000;
        private const int SubkeyLength = 32;
        private const byte IdentityHashVersion = 0x01;

        /// <summary>
        /// 内置管理员的固定口令哈希。
        /// 与 <c>IPasswordHasher&lt;User&gt;</c>（PasswordHasher）生成的哈希格式完全兼容，
        /// 登录时照常校验通过；区别只是盐值固定，可用作 HasData 的确定值。
        /// </summary>
        public static readonly string AdministratorPasswordHash = CreatePasswordHash(DefaultPassword);

        /// <summary>
        /// 按 ASP.NET Core Identity 的 V3 哈希格式生成口令哈希，字节布局（均为大端）：
        /// [0]=版本 | [1..4]=PRF | [5..8]=迭代次数 | [9..12]=盐长度 | 盐 | 子密钥。
        /// PasswordHasher.VerifyHashedPassword 会从哈希里读出盐长度与盐，因此可正常校验。
        /// </summary>
        private static string CreatePasswordHash(string password)
        {
            var prf = KeyDerivationPrf.HMACSHA512;
            var subkey = KeyDerivation.Pbkdf2(password, SeedSalt, prf, IterationCount, SubkeyLength);

            var hash = new byte[13 + SeedSalt.Length + subkey.Length];
            hash[0] = IdentityHashVersion;
            BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(1, 4), (uint)prf);
            BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(5, 4), IterationCount);
            BinaryPrimitives.WriteUInt32BigEndian(hash.AsSpan(9, 4), (uint)SeedSalt.Length);
            SeedSalt.CopyTo(hash, 13);
            subkey.CopyTo(hash, 13 + SeedSalt.Length);

            return Convert.ToBase64String(hash);
        }
    }
}
