import { GetTopIssueSelectedAsync, GetTopIssueAvailableAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'
const top_issue_selected = ref([]);
const top_issue_available = ref([]);

const filterAvailable = ref({
    Is_Active:''
  })
  const filterSelected = ref({
    Is_Active:''
  })



async function GetTopIssueAvailableByFilter() {
const result = await GetTopIssueAvailableAsync(
  filterAvailable.value.Is_Active
  )

if (result.is_success) {
  top_issue_available.value = result.top_issue_available
}
}
export { filterAvailable, top_issue_available, GetTopIssueAvailableByFilter }

async function GetTopIssueSelectedByFilter() {
  const result = await GetTopIssueSelectedAsync(
    filterSelected.value.Is_Active
    )

  if (result.is_success) {
    top_issue_selected.value = result.top_issue_selected
  }
}
export { filterSelected, top_issue_selected, GetTopIssueSelectedByFilter }