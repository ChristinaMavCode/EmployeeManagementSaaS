namespace EmployeeManagementSaaS.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Skill mapping
        CreateMap<Skill, SkillDto>();

        CreateMap<SkillDto, Skill>()
            .ForMember(dest => dest.CreationAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<CreateSkillCommand, Skill>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreationAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Employee mapping
        CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.FullName, opt => opt.Ignore());
        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.CreationAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}