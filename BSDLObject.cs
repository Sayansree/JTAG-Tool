using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
        public class BSDLObject
        {
            public string component_name { get; set; }
            public GenericParameter generic_parameter { get; set; }
            public List<LogicalPortDescription> logical_port_description { get; set; }
            public string standard_use_statement { get; set; }
            public string component_conformance_statement { get; set; }
            public List<DevicePackagePinMapping> device_package_pin_mappings { get; set; }
            public List<GroupedPortIdentification> grouped_port_identification { get; set; }
            public List<ScanPortIdentification> scan_port_identification { get; set; }
            public ComplianceEnableDescription compliance_enable_description { get; set; }
            public InstructionRegisterDescription instruction_register_description { get; set; }
            public OptionalRegisterDescription optional_register_description { get; set; }
            public List<RegisterAccessDescription> register_access_description { get; set; }
            public BoundaryScanRegisterDescription boundary_scan_register_description { get; set; }
            public object bsdl_extensions { get; set; }
            public object design_warning { get; set; }
            public object intest_description { get; set; }
            public object power_port_association_description { get; set; }
            public object register_assembly_description { get; set; }
            public object register_association_description { get; set; }
            public object register_constraints_description { get; set; }
            public object register_fields_description { get; set; }
            public object register_mnemonics_description { get; set; }
            public object runbist_description { get; set; }
            public object system_clock_description { get; set; }
            public object use_statement { get; set; }
        }

        public class BoundaryRegister
    {
        public string cell_number { get; set; }
        public CellInfo cell_info { get; set; }
    }

    public class BoundaryScanRegisterDescription
    {
        public FixedBoundaryStmts fixed_boundary_stmts { get; set; }
        public object segment_boundary_stmts { get; set; }
    }

    public class CellInfo
    {
        public CellSpec cell_spec { get; set; }
        public InputOrDisableSpec input_or_disable_spec { get; set; }
    }

    public class CellSpec
    {
        public string cell_name { get; set; }
        public string port_id { get; set; }
        public string function { get; set; }
        public string safe_bit { get; set; }
    }

    public class ComplianceEnableDescription
    {
        public CompliancePatterns compliance_patterns { get; set; }
    }

    public class CompliancePatterns
    {
        public List<string> compliance_port_list { get; set; }
        public List<string> pattern_list { get; set; }
    }

    public class DevicePackagePinMapping
    {
        public string pin_mapping_name { get; set; }
        public List<PinMap> pin_map { get; set; }
    }

    public class FixedBoundaryStmts
    {
        public string boundary_length { get; set; }
        public List<BoundaryRegister> boundary_register { get; set; }
    }

    public class GenericParameter
    {
        public string default_device_package_type { get; set; }
    }

    public class GroupedPortIdentification
    {
        public string twin_group_type { get; set; }
        public List<TwinGroupList> twin_group_list { get; set; }
        public List<object> parseinfo { get; set; }
    }

    public class InputOrDisableSpec
    {
        public string control_cell { get; set; }
        public string disable_value { get; set; }
        public string disable_result { get; set; }
    }

    public class InstructionCaptureList
    {
        public string instruction_name { get; set; }
        public object pattern { get; set; }
    }

    public class InstructionOpcode
    {
        public string instruction_name { get; set; }
        public List<string> opcode_list { get; set; }
    }

    public class InstructionRegisterDescription
    {
        public string instruction_length { get; set; }
        public List<InstructionOpcode> instruction_opcodes { get; set; }
        public List<string> instruction_capture { get; set; }
        public object instruction_private { get; set; }
    }

    public class LogicalPortDescription
    {
        public List<string> identifier_list { get; set; }
        public string pin_type { get; set; }
        public string port_dimension { get; set; }
    }

    public class OptionalRegisterDescription
    {
        public List<string> idcode_register { get; set; }
    }

    public class PinMap
    {
        public string port_name { get; set; }
        public List<string> pin_list { get; set; }
        public List<object> parseinfo { get; set; }
    }

    public class Register
    {
        public string reg_name { get; set; }
    }

    public class RegisterAccessDescription
    {
        public Register register { get; set; }
        public List<InstructionCaptureList> instruction_capture_list { get; set; }
    }

   

    public class ScanPortIdentification
    {
        public TapScanClock tap_scan_clock { get; set; }
        public string tap_scan_in { get; set; }
        public string tap_scan_mode { get; set; }
        public string tap_scan_out { get; set; }
        public string tap_scan_reset { get; set; }
    }

    public class TapScanClock
    {
        public string frequency { get; set; }
        public string halt_value { get; set; }
    }

    public class TwinGroupList
    {
        public string representative_port { get; set; }
        public string associated_port { get; set; }
        public List<object> parseinfo { get; set; }
    }


    public class BoardFile
    {
        public string boardName { get; set; }
        public int controllerCount { get; set; }
        public List<Controller> controllerArray { get; set; }

    }
    public class Controller
    {
        public string devid {  get; set; }
        public string deviceName { get; set; }
        //public string fileName { get; set; }
        public int BSLEN { get; set; }
        public int IRLEN { get; set; }
        public string EXTEXT { get; set; }
        public string flashType { get; set; }
        public List<BoardPins> pins { get; set; }

    }


    public class BoardPins
    {
        public int pinNumber { get; set; }
        public string pinName { get; set; }
        public string pinMap { get; set; }
        public bool selected { get; set; }
        public string Type { get; set; }
        public bool safe { get; set; }
        public PIN pin { get; set; }
        public string FlashInterface { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

}
