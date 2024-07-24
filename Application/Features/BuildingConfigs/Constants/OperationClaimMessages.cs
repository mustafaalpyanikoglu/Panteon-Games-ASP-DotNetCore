namespace Application.Features.BuildingConfigs.Constants;

public static class BuildingConfigMessages
{
    public const string AddedBuildingConfig = "added building config";
    public const string DeletedBuildingConfig = "deleted building config";
    public const string UpdatedBuildingConfig = "updated building config";
    public const string BuildingConfigNameAlreadyExists = "building config name already exists";
    public const string OperationFailed = "operation failed";
    public const string BuildingConfigAvaliable = "building config avaliable";
    public const string BuildingConfigNotFound = "building config not found";
    public const string BuildingCostMustBeGreaterThanZero = "Building cost must be greater than zero.";
    public const string ConstructionTimeMustBeBetween30And1800Seconds = "Construction time must be between 30 and 1800 seconds.";
    public const string BuildingTypeMustBeAValidEnumValue = "Building type must be a valid enum value.";
    public const string BuildingTypeAlreadyAdded = "This type has already been added and should not be visible in the combobox.";

}
