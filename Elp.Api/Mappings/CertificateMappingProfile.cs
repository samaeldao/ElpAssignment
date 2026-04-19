using AutoMapper;
using Elp.Api.Models;
using Elp.Application.Certificates.Commands.CreateCertificate;
using Elp.Application.Certificates.Queries.GetCertificateById;
using Elp.Application.Certificates.Queries.GetCodebookItems;
using Elp.Application.Certificates.Queries.GetCodebooks;
using Elp.Domain.Entities;

namespace Elp.Api.Mappings;

public class CertificateMappingProfile : Profile
{
    public CertificateMappingProfile()
    {
        // INCOMING: Czech API Contract -> English Command (POST)
        CreateMap<PosudekRoCreateDto, CreateCertificateCommand>()
            .ForMember(dest => dest.PersonalId, opt => opt.MapFrom(src => src.Rid))
            .ForMember(dest => dest.MedicalProfessionalId, opt => opt.MapFrom(src => src.KrzpId.HasValue ? src.KrzpId.Value : Guid.Empty))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.DatumVystaveni))
            .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.StavPosudku.Kod));

        // OUTGOING: English Application DTO -> Czech API Contract (GET)
        CreateMap<CertificateDto, PosudekRoDetailDto>()
            .ForMember(dest => dest.Rid, opt => opt.MapFrom(src => src.PersonalId))
            .ForMember(dest => dest.KrzpId, opt => opt.MapFrom(src => src.MedicalProfessionalId.ToString()))
            .ForMember(dest => dest.DatumVystaveni, opt => opt.MapFrom(src => src.IssueDate))
            .ForPath(dest => dest.StavPosudku.Kod, opt => opt.MapFrom(src => src.StatusCode))
            // Hardcoding version for the contract response, assuming 1.0 if not stored in DB
            .ForPath(dest => dest.StavPosudku.Verze, opt => opt.MapFrom(src => "1.0"));

        CreateMap<CodebookDto, CiselnikDto>()
            .ForMember(dest => dest.Kod, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Nazev, opt => opt.MapFrom(src => src.Name));

        CreateMap<CodebookItemDto, CiselnikPolozkaDto>()
            .ForMember(dest => dest.Kod, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Popis, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Aktivni, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<DriverFitnessCertificate, CertificateDto>();

        CreateMap<CertificateDto, PosudekRoDetailDto>()
            .ForMember(dest => dest.Rid, opt => opt.MapFrom(src => src.PersonalId))
            .ForMember(dest => dest.KrzpId, opt => opt.MapFrom(src => src.MedicalProfessionalId.ToString()))
            .ForMember(dest => dest.DatumVystaveni, opt => opt.MapFrom(src => src.IssueDate))
            .ForPath(dest => dest.StavPosudku.Kod, opt => opt.MapFrom(src => src.StatusCode))
            .ForPath(dest => dest.StavPosudku.Verze, opt => opt.MapFrom(src => "1.0"))
            .ForMember(dest => dest.RowVersion,
                opt => opt.MapFrom(src => src.RowVersion != null
                    ? Convert.ToBase64String(src.RowVersion)
                    : null));
    }
}