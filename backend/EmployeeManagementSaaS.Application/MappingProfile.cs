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
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.FullName, opt => opt.Ignore())
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                src.Skills != null
                    ? string.Join(", ", src.Skills.Select(s => s.Name))
                    : string.Empty
            ));
        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Skills, opt => opt.Ignore())
            .ForMember(dest => dest.CreationAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}