import os
import re

def seed_faults_in_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Find functions using a simple regex (looks for public/private/protected [type] [name]([args]) {)
    # This is a bit rough but works for C#
    function_pattern = r'(?:public|private|protected|internal)\s+[\w<>, \[\]]+\s+\w+\s*\(.*?\)\s*\{'
    
    parts = re.split(f'({function_pattern})', content)
    
    new_content = parts[0]
    fault_count_total = 0
    
    # parts[i] is the function signature, parts[i+1] is the function body (starts with the rest of the file)
    # Actually split gives: [pre, sig1, body1_start, sig2, body2_start, ...]
    
    i = 1
    while i < len(parts):
        sig = parts[i]
        body_and_rest = parts[i+1]
        
        # Find the matching closing brace for this function
        brace_level = 1
        body_end_idx = -1
        for idx, char in enumerate(body_and_rest):
            if char == '{': brace_level += 1
            if char == '}': brace_level -= 1
            if brace_level == 0:
                body_end_idx = idx
                break
        
        if body_end_idx != -1:
            body = body_and_rest[:body_end_idx]
            rest = body_and_rest[body_end_idx:]
            
            # Inject 5 faults into the body
            faults = [
                (r'==', '!=', 1),
                (r'!=', '==', 1),
                (r'\|\|', '&&', 1),
                (r'&&', '||', 1),
                (r'true', 'false', 1),
                (r'false', 'true', 1),
                (r'throw new', '// throw new', 1),
                (r'return ', 'return !', 1), # Very aggressive
                (r'if \(', 'if (!', 1),     # Very aggressive
                (r'\.Add', '.Remove', 1)
            ]
            
            mutated_body = body
            applied_in_this_func = 0
            
            # Simple replacement loop
            for pattern, replacement, limit in faults:
                if applied_in_this_func >= 5: break
                
                matches = list(re.finditer(pattern, mutated_body))
                if matches:
                    # Apply a few replacements
                    for _ in range(min(len(matches), 5 - applied_in_this_func)):
                        mutated_body = re.sub(pattern, replacement, mutated_body, count=1)
                        applied_in_this_func += 1
                        fault_count_total += 1
            
            # Add a comment about the seeding
            mutated_body = f"\n            /* SEEDED FAULTS: {applied_in_this_func} faults injected here */\n" + mutated_body
            
            new_content += sig + mutated_body + rest[:1] # rest[:1] is the closing brace
            parts[i+1] = rest[1:] # Update rest for next iteration
        else:
            new_content += sig + body_and_rest
        
        i += 2
        
    with open(file_path, 'w', encoding='utf-8') as f:
        f.write(new_content)
    
    return fault_count_total

def main():
    base_dir = r"c:\University\SEMESTER # 8\SMM\Project\FASTSocietiesSystem"
    targets = ['BLL', 'DAL']
    
    total_faults = 0
    for target in targets:
        target_path = os.path.join(base_dir, target)
        for root, dirs, files in os.walk(target_path):
            for file in files:
                if file.endswith('.cs'):
                    file_path = os.path.join(root, file)
                    print(f"Processing {file_path}...")
                    faults = seed_faults_in_file(file_path)
                    total_faults += faults
                    print(f"Injected {faults} faults.")

    print(f"\nTotal faults injected: {total_faults}")

if __name__ == "__main__":
    main()
