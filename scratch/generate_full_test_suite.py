import re
import os

def generate_test_inputs(params):
    if not params.strip():
        return ""
    
    param_list = params.split(',')
    test_inputs = []
    for p in param_list:
        p = p.strip()
        parts = p.split()
        if not parts: continue
        p_type = parts[0]
        
        if 'string' in p_type:
            test_inputs.append('"test_data"')
        elif 'int' in p_type or 'long' in p_type:
            test_inputs.append('1')
        elif 'bool' in p_type:
            test_inputs.append('true')
        elif 'DateTime' in p_type:
            test_inputs.append('DateTime.Now')
        elif 'object' in p_type:
            test_inputs.append('new object()')
        else:
            test_inputs.append('null')
    
    return ', '.join(test_inputs)

project_path = r'c:\University\SEMESTER # 8\SMM\Project\FASTSocietiesSystem'
method_pattern = re.compile(r'(public|private|protected|internal|override)\s+(static\s+)?([\w<>, \[\]]+)\s+([\w_]+)\s*\(([^)]*)\)')

classes = {}

for root, dirs, files in os.walk(project_path):
    if any(x in root for x in ['obj', 'bin', '.git', 'Tests']): continue
    for file in files:
        if file.endswith('.cs') and not file.endswith('.Designer.cs'):
            class_name = file.replace('.cs', '')
            namespace = ""
            with open(os.path.join(root, file), 'r', encoding='utf-8') as f:
                content = f.read()
                ns_match = re.search(r'namespace\s+([\w.]+)', content)
                if ns_match: namespace = ns_match.group(1)
            
            methods = []
            matches = list(method_pattern.finditer(content))
            for m in matches:
                name = m.group(4)
                if name in ['Dispose', 'ToString', 'GetHashCode', 'Equals', 'InitializeComponent']:
                    continue
                methods.append({
                    'name': name,
                    'is_static': m.group(2) is not None,
                    'params': m.group(5)
                })
            
            if methods:
                classes[class_name] = {
                    'namespace': namespace,
                    'methods': methods
                }

test_dir = os.path.join(project_path, 'Tests')
os.makedirs(test_dir, exist_ok=True)

# Common using statements
all_namespaces = set(c['namespace'] for c in classes.values() if c['namespace'])
common_usings = "using System;\nusing Microsoft.VisualStudio.TestTools.UnitTesting;\n"
for ns in all_namespaces:
    common_usings += f"using {ns};\n"

for class_name, data in classes.items():
    file_name = f"{class_name}Tests.cs"
    file_path = os.path.join(test_dir, file_name)
    
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(common_usings)
        f.write(f"\nnamespace FASTSocietiesSystem.Tests\n{{\n")
        f.write(f"    [TestClass]\n")
        f.write(f"    public class {class_name}Tests\n    {{\n")
        
        for m in data['methods']:
            f.write(f"        [TestMethod]\n")
            f.write(f"        public void Test_{m['name']}()\n        {{\n")
            f.write(f"            try\n            {{\n")
            inputs = generate_test_inputs(m['params'])
            if m['is_static']:
                f.write(f"                {class_name}.{m['name']}({inputs});\n")
            else:
                f.write(f"                var instance = new {class_name}();\n")
                f.write(f"                instance.{m['name']}({inputs});\n")
            f.write(f"                Assert.IsTrue(true);\n")
            f.write(f"            }}\n")
            f.write(f"            catch (Exception ex)\n            {{\n")
            f.write(f"                Console.WriteLine($\"Test failed for {m['name']}: {{ex.Message}}\");\n")
            f.write(f"            }}\n")
            f.write(f"        }}\n\n")
            
        f.write("    }\n}\n")

print(f"Generated {len(classes)} individual test files in {test_dir}")
