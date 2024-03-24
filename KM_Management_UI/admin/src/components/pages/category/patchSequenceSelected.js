import { PatchSequenceTopIssueSelectedAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const editSequence = ref({
    Current_Sequence:'',
    New_Sequence: '',
})

const errorSequence = ref({
  top_issue: { isError: false, message: '' },
})


async function HandleRePublish() {

  const patchSequenceSelected = await PatchSequenceTopIssueSelectedAsync(
    editSequence.value.Current_Sequence,
    editSequence.value.New_Sequence,
  )

  if (!patchSequenceSelected.is_success) {
    if (patchSequenceSelected.error.statuscode === 400) {
      errorSequence.value.top_issue.isError = true
      errorSequence.value.top_issue.message = patchSequenceSelected.error.message
    }
    return patchSequenceSelected.error.statuscode
  }
  return true
}

export { editSequence, errorSequence, HandleRePublish }