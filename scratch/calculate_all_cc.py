import re
import os

def calculate_cc(code):
    patterns = [
        r'\bif\b', r'\bwhile\b', r'\bfor\b', r'\bforeach\b',
        r'\bcase\b', r'\bcatch\b', r'\&\&', r'\|\|', r'\?\.', r'\?\?'
    ]
    count = 1
    for pattern in patterns:
        count += len(re.findall(pattern, code))
    return count

def generate_test_case(params):
    if not params.strip():
        return "No inputs (Void/Empty)"
    
    param_list = params.split(',')
    test_inputs = []
    for p in param_list:
        p = p.strip()
        if 'string' in p:
            if 'email' in p.lower(): test_inputs.append('"user@fast.com"')
            elif 'pass' in p.lower(): test_inputs.append('"Pass123!"')
            elif 'name' in p.lower(): test_inputs.append('"John Doe"')
            else: test_inputs.append('"SampleData"')
        elif 'int' in p or 'long' in p:
            if 'id' in p.lower(): test_inputs.append('101')
            else: test_inputs.append('5')
        elif 'bool' in p:
            test_inputs.append('true')
        elif 'DateTime' in p:
            test_inputs.append('2024-12-01')
        else:
            test_inputs.append('objRef')
    
    return f"Input: ({', '.join(test_inputs)})"

project_path = r'c:\University\SEMESTER # 8\SMM\Project\FASTSocietiesSystem'
all_methods = []
# Capture modifiers, name, and parameters
method_pattern = re.compile(r'(public|private|protected|internal|override)\s+[\w<>, \[\]]+\s+([\w_]+)\s*\(([^)]*)\)')

for root, dirs, files in os.walk(project_path):
    if any(x in root for x in ['obj', 'bin', '.git']): continue
    for file in files:
        if file.endswith('.cs') and not file.endswith('.Designer.cs'):
            class_name = file.replace('.cs', '')
            with open(os.path.join(root, file), 'r', encoding='utf-8') as f:
                content = f.read()
            matches = list(method_pattern.finditer(content))
            for i in range(len(matches)):
                start = matches[i].start()
                end = matches[i+1].start() if i+1 < len(matches) else len(content)
                method_body = content[start:end]
                if '{' in method_body:
                    cc = calculate_cc(method_body)
                    params = matches[i].group(3)
                    test_case = generate_test_case(params)
                    all_methods.append((f"{class_name}.{matches[i].group(2)}", cc, test_case))

all_methods.sort(key=lambda x: x[0])
filtered = [m for m in all_methods if m[0].split('.')[1] not in ['Dispose', 'ToString', 'GetHashCode', 'Equals']]

output_file = r'c:\University\SEMESTER # 8\SMM\Project\FASTSocietiesSystem\scratch\full_cc_table.tex'
with open(output_file, 'w', encoding='utf-8') as f:
    for name, cc, test in filtered:
        escaped_name = name.replace('.', r'.\allowbreak ').replace('_', r'\_')
        escaped_name = escaped_name.replace(r'\_', r'\_ \allowbreak ')
        # Escape special chars in test cases for LaTeX
        escaped_test = test.replace('_', r'\_').replace('"', r'\"')
        f.write(f"{escaped_name} & {cc} & {escaped_test} \\\\ \\addlinespace\n")

print(f"Generated {len(filtered)} test cases in {output_file}")
