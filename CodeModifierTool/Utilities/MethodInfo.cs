using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class MethodInfo {
	
	public MethodInfo(string name, int startIndex, int endIndex) {
		Name = name;
		StartIndex = startIndex;
		EndIndex = endIndex;
	}
	public string Name { get; set; }
	public int StartIndex { get; set; }
	public int EndIndex { get; set; }
	public bool IsNested { get; set; }
	public List<MethodInfo> NestedMethods { get; set; } = new List<MethodInfo>();
	
	public bool HasNestedMethod(string methodMatch, int index) {
		var nesteds = NestedMethods.LWhere(s => s.Name == methodMatch || s.StartIndex == index);
		return nesteds.Count > 0;
	}
	
	public bool IsEndIndex(int index) {
		return this.EndIndex <= index;
	}
}
