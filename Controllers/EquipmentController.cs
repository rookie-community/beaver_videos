using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace BeaverVideos.Controllers
{
    [Authorize]
    public class EquipmentController : AbpController
    {
        //private readonly ILogger<EquipmentController> _logger;

        //public EquipmentController(IFreeSql freeSql, IOTCloudService cloudService, CommonService commonService, ILogger<EquipmentController> logger)
        //{
        //    _freeSql = freeSql;
        //    _cloudService = cloudService;
        //    _commonService = commonService;
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    try
        //    {
        //        var area = _freeSql.Queryable<LocalConfig>().Where(x => x.Code == ConfigType.Area.ToString()).First(x => x.Value);
        //        var model = new EquipmentQueryModel
        //        {
        //            DestinationArea = area
        //        };
        //        return View(model);
        //    }
        //    catch (Exception)
        //    {
        //        return View();
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Load(EquipmentQueryModel model, int page, int limit)
        //{
        //    try
        //    {
        //        var query = _freeSql.Queryable<Equipment>().Where(GetFilter(model));
        //        var result = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
        //        var total = await query.CountAsync();
        //        return Ok(new ResultDto<List<Equipment>>
        //        {
        //            Code = 0,
        //            Count = total,
        //            Data = result
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new ResultDto<List<Equipment>>
        //        {
        //            Code = 500,
        //            Msg = ex.Message
        //        });
        //    }
        //}

        //private static Expression<Func<Equipment, bool>> GetFilter(EquipmentQueryModel model)
        //{
        //    var filter = PredicateBuilder.New<Equipment>(true);
        //    if (!string.IsNullOrWhiteSpace(model.Code))
        //    {
        //        filter = filter.And(x => x.Code.Contains(model.Code));
        //    }
        //    if (!string.IsNullOrWhiteSpace(model.Name))
        //    {
        //        filter = filter.And(x => x.Name.Contains(model.Name));
        //    }
        //    if (model.EquipmentTypeId != 0)
        //    {
        //        filter = filter.And(x => x.EquipmentTypeId == model.EquipmentTypeId);
        //    }
        //    if (!string.IsNullOrWhiteSpace(model.DestinationArea))
        //    {
        //        filter = filter.And(x => x.DestinationArea == model.DestinationArea);
        //    }
        //    return filter;
        //}

        //[HttpGet]
        //public async Task<IActionResult> SyncData()
        //{
        //    using var uow = _freeSql.CreateUnitOfWork();
        //    try
        //    {
        //        if (_commonService.IsStart)
        //        {
        //            return Ok(new ResultDto
        //            {
        //                Code = 500,
        //                Msg = "服务正在运行，请在菜单栏的控制台页码点击“停止”后，再进行数据同步操作！"
        //            });
        //        }

        //        var clientIdValue = await _freeSql.Queryable<LocalConfig>().Where(x => x.Code == ConfigType.ClientId.ToString()).FirstAsync(x => x.Value);
        //        _ = Guid.TryParse(clientIdValue, out var clientId);
        //        if (clientId == default)
        //        {
        //            return Ok(new ResultDto
        //            {
        //                Code = 500,
        //                Msg = "当前客户端未配置“ClientId”，请检查配置信息后再试！"
        //            });
        //        }

        //        var result = await _cloudService.GetClientEquipmentDataAsync(clientId);
        //        if (!result.Success)
        //        {
        //            return Ok(new ResultDto
        //            {
        //                Code = 500,
        //                Msg = $"同步数据失败：从服务端获取数据失败，{result.Msg}"
        //            });
        //        }

        //        var user = HttpContext.User.Identity?.Name ?? string.Empty;

        //        //事务操作
        //        using var equipmentTypeRepository = uow.GetRepository<EquipmentType>();
        //        using var equipmentRepository = uow.GetRepository<Equipment>();
        //        using var equipmentPropRepository = uow.GetRepository<EquipmentProp>();
        //        using var equipmentTypePropTemplateRepository = uow.GetRepository<EquipmentTypePropTemplate>();
        //        using var communicationConfigRepository = uow.GetRepository<CommunicationConfig>();

        //        //清空本地数据
        //        await equipmentTypePropTemplateRepository.DeleteAsync(x => true);
        //        await equipmentPropRepository.DeleteAsync(x => true);
        //        await equipmentRepository.DeleteAsync(x => true);
        //        await equipmentTypeRepository.DeleteAsync(x => true);

        //        //同步设备类型
        //        foreach (var equipmentType in result.Data)
        //        {
        //            equipmentType.Id = 0;
        //            equipmentType.Created = DateTime.Now;
        //            equipmentType.CreatedBy = user;
        //            equipmentType.Updated = null;
        //            equipmentType.UpdatedBy = string.Empty;
        //            equipmentType.Id = (await equipmentTypeRepository.InsertAsync(equipmentType)).Id;
        //            //同步设备模板
        //            var equipmentTypePropTemplates = new List<EquipmentTypePropTemplate>();
        //            foreach (var equipmentTypePropTemplate in equipmentType.EquipmentTypePropTemplates)
        //            {
        //                equipmentTypePropTemplate.Id = 0;
        //                equipmentTypePropTemplate.EquipmentTypeId = equipmentType.Id;
        //                equipmentTypePropTemplate.Created = DateTime.Now;
        //                equipmentTypePropTemplate.CreatedBy = user;
        //                equipmentTypePropTemplate.Updated = null;
        //                equipmentTypePropTemplate.UpdatedBy = string.Empty;
        //                equipmentTypePropTemplates.Add(equipmentTypePropTemplate);
        //            }
        //            await equipmentTypePropTemplateRepository.InsertAsync(equipmentTypePropTemplates);

        //            //同步设备
        //            foreach (var equipment in equipmentType.Equipments)
        //            {
        //                equipment.Id = 0;
        //                equipment.EquipmentTypeId = equipmentType.Id;
        //                equipment.Created = DateTime.Now;
        //                equipment.CreatedBy = user;
        //                equipment.Updated = null;
        //                equipment.UpdatedBy = string.Empty;
        //                equipment.Id = (await equipmentRepository.InsertAsync(equipment)).Id;
        //                var equipmentProps = new List<EquipmentProp>();
        //                foreach (var equipmentProp in equipment.EquipmentProps)
        //                {
        //                    equipmentProp.Id = 0;
        //                    equipmentProp.EquipmentId = equipment.Id;
        //                    equipmentProp.Value = string.Empty;
        //                    equipmentProp.Created = DateTime.Now;
        //                    equipmentProp.CreatedBy = user;
        //                    equipmentProp.Updated = null;
        //                    equipmentProp.UpdatedBy = string.Empty;
        //                    equipmentProps.Add(equipmentProp);
        //                }
        //                await equipmentPropRepository.InsertAsync(equipmentProps);
        //            }

        //            //新增默认连接对象
        //            var equipmentIps = equipmentType.Equipments.Select(x => x.IP).Distinct().ToList();
        //            var communicationConfigs = new List<CommunicationConfig>();
        //            foreach (var ipAddress in equipmentIps)
        //            {
        //                if (communicationConfigRepository.Where(x => x.IpAddress == ipAddress).Any())
        //                {
        //                    continue;
        //                }
        //                var communicationConfig = new CommunicationConfig
        //                {
        //                    Code = Guid.NewGuid().ToString()[..8],
        //                    Name = Guid.NewGuid().ToString()[..8],
        //                    CommunicationType = CommunicationTypeConst.Siemens_S1500,
        //                    Port = 102,
        //                    IpAddress = ipAddress,
        //                    Disable = false,
        //                    Created = DateTime.Now,
        //                    CreatedBy = user,
        //                };
        //                communicationConfigs.Add(communicationConfig);
        //            }
        //            await communicationConfigRepository.InsertAsync(communicationConfigs);
        //        }
        //        uow.Commit();
        //        var msg = "数据同步成功";
        //        _logger.LogInformation(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 0,
        //            Msg = msg
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        uow.Rollback();
        //        var msg = $"数据同步失败：{ex.Message}";
        //        _logger.LogError(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 500,
        //            Msg = msg
        //        });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        //public async Task<IActionResult> Add(Equipment model)
        //{
        //    try
        //    {
        //        model.CreatedBy = HttpContext.User.Identity?.Name ?? string.Empty;
        //        model.Created = DateTime.Now;
        //        await _freeSql.Insert<Equipment>(model).ExecuteAffrowsAsync();
        //        var msg = "新增成功";
        //        _logger.LogInformation(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 0,
        //            Msg = msg
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = $"新增失败：{ex.Message}";
        //        _logger.LogError(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 500,
        //            Msg = msg
        //        });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateStatus(Equipment model)
        //{
        //    try
        //    {
        //        var user = HttpContext.User.Identity?.Name ?? string.Empty;
        //        await _freeSql.Update<Equipment>(model.Id).Set(x => new Equipment
        //        {
        //            Disable = model.Disable,
        //            UpdatedBy = user,
        //            Updated = DateTime.Now
        //        }).ExecuteAffrowsAsync();

        //        var msg = $"更新状态成功";
        //        _logger.LogInformation(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 0,
        //            Msg = msg
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = $"更新状态失败：{ex.Message}";
        //        _logger.LogError(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 500,
        //            Msg = msg
        //        });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        //public async Task<IActionResult> Update(Equipment model)
        //{
        //    try
        //    {
        //        var user = HttpContext.User.Identity?.Name ?? string.Empty;
        //        await _freeSql.Update<Equipment>(model.Id).Set(x => new Equipment
        //        {
        //            Code = model.Code,
        //            Name = model.Name,
        //            EquipmentTypeId = model.EquipmentTypeId,
        //            ConnectName = model.ConnectName,
        //            IP = model.IP,
        //            Disable = model.Disable,
        //            FactoryCode = model.FactoryCode,
        //            ProjectCode = model.ProjectCode,
        //            DestinationArea = model.DestinationArea,
        //            Description = model.Description,
        //            UpdatedBy = user,
        //            Updated = DateTime.Now
        //        }).ExecuteAffrowsAsync();

        //        var msg = $"更新成功";
        //        _logger.LogInformation(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 0,
        //            Msg = msg
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = $"更新失败：{ex.Message}";
        //        _logger.LogError(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 500,
        //            Msg = msg
        //        });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = $"{nameof(RoleType.Admin)}")]
        //public async Task<IActionResult> Delete(int[] ids)
        //{
        //    try
        //    {
        //        await _freeSql.Delete<Equipment>().Where(x => ids.Contains(x.Id)).ExecuteAffrowsAsync();
        //        var msg = "删除成功";
        //        _logger.LogInformation(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 0,
        //            Msg = msg
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = $"删除失败：{ex.Message}";
        //        _logger.LogError(msg);
        //        return Ok(new ResultDto
        //        {
        //            Code = 500,
        //            Msg = msg
        //        });
        //    }
        //}
    }
}
